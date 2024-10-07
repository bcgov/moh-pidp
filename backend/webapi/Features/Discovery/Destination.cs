namespace Pidp.Features.Discovery;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Models.Lookups;

public class Destination
{
    public class Query : IQuery<Model>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public enum DestinationCode
        {
            Demographics = 1,
            UserAccessAgreement,
            LicenceDeclaration,
            Portal
        }
        public DestinationCode Destination { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, Model>
    {
        private readonly PidpDbContext context = context;

        public async Task<Model> HandleAsync(Query query)
        {
            var party = await this.context.Parties
                .Where(party => party.Id == query.PartyId)
                .Select(party => new
                {
                    DemographicsCompleted = party.Email != null && party.Phone != null,
                    UaaCompleted = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.UserAccessAgreement),
                    LicenceResolved = party.Credentials.All(credential => credential.IdentityProvider != IdentityProviders.BCServicesCard)
                        || party.Cpn != null
                        || (party.LicenceDeclaration != null && party.LicenceDeclaration.IsUnlicensed)
                })
                .SingleAsync();

            return new Model
            {
                Destination = party switch
                {
                    { DemographicsCompleted: false } => Model.DestinationCode.Demographics,
                    { UaaCompleted: false } => Model.DestinationCode.UserAccessAgreement,
                    { LicenceResolved: false } => Model.DestinationCode.LicenceDeclaration,
                    _ => Model.DestinationCode.Portal
                }
            };
        }
    }
}
