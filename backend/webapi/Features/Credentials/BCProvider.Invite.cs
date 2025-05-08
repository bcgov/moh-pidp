namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
using static Pidp.Features.CommonHandlers.UpdateBcProviderAttributesConsumer;

public class BCProviderInvite
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string UserPrincipalName { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.UserPrincipalName).NotEmpty();
        }
    }

    public class CommandHandler(
        IBCProviderClient bcProviderClient,
        IBus bus,
        IClock clock,
        IPlrClient plrClient,
        PidpConfiguration config,
        PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient client = bcProviderClient;
        private readonly IBus bus = bus;
        private readonly IClock clock = clock;
        private readonly IPlrClient plrClient = plrClient;
        private readonly PidpDbContext context = context;
        private readonly string clientId = config.BCProviderClient.ClientId;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var createdUpn = await this.client.SendInvite(command.UserPrincipalName);
            if (createdUpn == null)
            {
                return DomainResult.Failed();
            }

            this.context.InvitedEntraAccounts.Add(new InvitedEntraAccount
            {
                PartyId = command.PartyId,
                UserPrincipalName = createdUpn,
                InvitedAt = this.clock.GetCurrentInstant(),
            });
            await this.context.SaveChangesAsync();

            var party = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.FirstName,
                    party.LastName,
                    party.Cpn,
                    party.Email,
                    Hpdid = party.Credentials
                        .Select(cred => cred.Hpdid)
                        .Single(hpdid => hpdid != null),
                    UaaAgreementDate = party.AccessRequests
                        .Where(request => request.AccessTypeCode == AccessTypeCode.UserAccessAgreement)
                        .Select(request => request.RequestedOn)
                        .SingleOrDefault()
                })
                .SingleAsync();

            var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);
            var endorsingCpns = await this.context.ActiveEndorsingParties(command.PartyId)
                .Select(party => party.Cpn)
                .ToListAsync();
            var endorsementPlrDigest = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            var attributes = new BCProviderAttributes(this.clientId)
                .SetHpdid(party.Hpdid!)
                .SetLoa(3)
                .SetPidpEmail(party.Email!)
                .SetIsRnp(plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding)
                .SetIsMd(plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding)
                .SetIsPharm(plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding)
                .SetIsMoa(!plrStanding.HasGoodStanding && endorsementPlrDigest.HasGoodStanding)
                .SetEndorserData(endorsementPlrDigest
                    .WithGoodStanding()
                    .With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes)
                    .Cpns);

            if (party.Cpn != null)
            {
                attributes.SetCpn(party.Cpn);
            }
            if (party.UaaAgreementDate != default)
            {
                attributes.SetUaaDate(party.UaaAgreementDate.ToDateTimeOffset());
            }

            await this.bus.Publish(new UpdateBcProviderAttributes(createdUpn, attributes.AsAdditionalData()));

            return DomainResult.Success();
        }
    }
}
