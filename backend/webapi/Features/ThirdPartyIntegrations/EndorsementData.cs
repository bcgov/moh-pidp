namespace Pidp.Features.ThirdPartyIntegrations;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Plr;

// Currently used by DMFT
public class EndorsementData
{
    public class Query : IQuery<IDomainResult<List<Model>>>
    {
        public string Hpdid { get; set; } = string.Empty;
    }

    public class Model
    {
        public string? Hpdid { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<LicenceInformation> Licences { get; set; } = [];

        public class LicenceInformation
        {
            public string? IdentifierType { get; set; }
            public string? StatusCode { get; set; }
            public string? StatusReasonCode { get; set; }
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Hpdid).NotEmpty();
    }

    public class QueryHandler(IPlrClient client, PidpDbContext context) : IQueryHandler<Query, IDomainResult<List<Model>>>
    {
        private readonly IPlrClient client = client;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult<List<Model>>> HandleAsync(Query query)
        {
            query.Hpdid = query.Hpdid.EndsWith("@bcsc", StringComparison.Ordinal)
                ? query.Hpdid
                : $"{query.Hpdid}@bcsc";

            var partyId = await this.context.Credentials
                .Where(credential => credential.Hpdid == query.Hpdid)
                .Select(credential => credential.PartyId)
                .SingleOrDefaultAsync();

            if (partyId == default)
            {
                return DomainResult.NotFound<List<Model>>();
            }

            var dtos = await this.context.ActiveEndorsingParties(partyId)
                .Select(party => new
                {
                    Hpdid = party.Credentials
                        .SingleOrDefault(credential => credential.IsBcServicesCard)!
                        .IdpId,
                    party.Cpn,
                    party.FirstName,
                    party.LastName,
                    party.Email
                })
                .ToListAsync();

            var licences = await this.client.GetRecordsAsync(dtos.Select(dto => dto.Cpn).ToArray());

            return DomainResult.Success(dtos.Select(dto => new Model
            {
                Hpdid = dto.Hpdid,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email!,
                Licences = licences?
                    .Where(licence => licence.Cpn == dto.Cpn)
                    .Select(licence => new Model.LicenceInformation
                    {
                        IdentifierType = licence.IdentifierType,
                        StatusCode = licence.StatusCode,
                        StatusReasonCode = licence.StatusReasonCode
                    })
                    .ToList() ?? []
            })
            .ToList());
        }
    }
}
