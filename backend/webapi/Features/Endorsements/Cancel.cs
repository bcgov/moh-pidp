namespace Pidp.Features.Endorsements;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;

public class Cancel
{
    public class Command : ICommand<IDomainResult>
    {
        public int EndorsementId { get; set; }
        public int PartyId { get; set; }
    }

    public class ComandValidator : AbstractValidator<Command>
    {
        public ComandValidator()
        {
            this.RuleFor(x => x.EndorsementId).GreaterThan(0);
            this.RuleFor(x => x.PartyId).GreaterThan(0);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient bcProviderClient;
        private readonly IPlrClient plrClient;
        private readonly PidpConfiguration config;
        private readonly PidpDbContext context;

        public CommandHandler(
            IBCProviderClient bcProviderClient,
            IPlrClient plrClient,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.bcProviderClient = bcProviderClient;
            this.config = config;
            this.context = context;
            this.plrClient = plrClient;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsement = await this.context.EndorsementRelationships
                .Where(relationship => relationship.EndorsementId == command.EndorsementId
                    && relationship.PartyId == command.PartyId)
                .Select(relationship => relationship.Endorsement)
                .SingleOrDefaultAsync();

            if (endorsement == null)
            {
                return DomainResult.NotFound();
            }

            endorsement.Active = false;
            await this.context.SaveChangesAsync();

            var unLicencedParty = await this.context.EndorsementRelationships
                .Where(relationship => relationship.EndorsementId == command.EndorsementId
                    && string.IsNullOrWhiteSpace(relationship.Party!.Cpn))
                .Select(relationship => new
                {
                    relationship.PartyId,
                    UserPrincipalName = relationship.Party!.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.IdpId)
                        .SingleOrDefault(),
                })
                .SingleAsync();

            if (!string.IsNullOrWhiteSpace(unLicencedParty.UserPrincipalName))
            {
                var endorsingCpns = await this.context.ActiveEndorsementRelationships(unLicencedParty.PartyId)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                var endorseePlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

                var endorseeIsMoa = endorseePlrStanding.HasGoodStanding;
                var endorseeBcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsMoa(endorseeIsMoa);
                await this.bcProviderClient.UpdateAttributes(unLicencedParty.UserPrincipalName, endorseeBcProviderAttributes.AsAdditionalData());
            }

            return DomainResult.Success();
        }
    }
}
