namespace Pidp.Features.ThirdPartyIntegrations;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;

// Currently used by DMFT
public class EndorsementData
{
    public class Query : IQuery<IDomainResult<List<Model>>>
    {
        public string Hpdid { get; set; } = string.Empty;
    }

    public class Model
    {
        public string? Hpdid { get; set; } = string.Empty;
        public List<LicenceInformation> Licences { get; set; } = new();

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

    public class QueryHandler : IQueryHandler<Query, IDomainResult<List<Model>>>
    {
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public QueryHandler(IPlrClient client, PidpDbContext context)
        {
            this.client = client;
            this.context = context;
        }

        public async Task<IDomainResult<List<Model>>> HandleAsync(Query query)
        {
            var partyId = await this.context.Credentials
                .Where(credential => credential.Hpdid!.Replace("@bcsc", "") == query.Hpdid)
                .Select(credential => credential.PartyId)
                .SingleOrDefaultAsync();

            if (partyId == default)
            {
                return DomainResult.NotFound<List<Model>>();
            }

            var dtos = await this.context.Endorsements
                .Where(endorsement => endorsement.Active
                    && endorsement.EndorsementRelationships.Any(relationship => relationship.PartyId == partyId))
                .SelectMany(endorsement => endorsement.EndorsementRelationships)
                .Where(relationship => relationship.PartyId != partyId)
                .Select(relationship => new Dto
                {
                    BcscCredential = relationship.Party!.Credentials
                        .SingleOrDefault(credential => credential.IsBcServicesCard),
                    Cpn = relationship.Party!.Cpn
                })
                .ToListAsync();

            var licences = await this.client.GetRecordsAsync(dtos.Select(dto => dto.Cpn).ToArray());

            return DomainResult.Success(dtos.Select(dto => new Model
            {
                Hpdid = dto.BcscCredential?.IdpId,
                Licences = licences?
                    .Where(licence => licence.Cpn == dto.Cpn)
                    .Select(licence => new Model.LicenceInformation
                    {
                        IdentifierType = licence.IdentifierType,
                        StatusCode = licence.StatusCode,
                        StatusReasonCode = licence.StatusReasonCode
                    })
                    .ToList() ?? new()
            })
            .ToList());
        }
    }

    private class Dto
    {
        public Credential? BcscCredential { get; set; }
        public string? Cpn { get; set; }
    }
}
