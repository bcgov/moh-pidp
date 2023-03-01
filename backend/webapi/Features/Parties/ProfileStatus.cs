namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Security.Claims;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
using static Pidp.Features.Parties.ProfileStatus.Model;

public partial class ProfileStatus
{
    public class Command : ICommand<Model>
    {
        public int Id { get; set; }
        [JsonIgnore]
        public ClaimsPrincipal? User { get; set; }

        public Command WithUser(ClaimsPrincipal user)
        {
            this.User = user;
            return this;
        }
    }

    public partial class Model
    {
        [JsonConverter(typeof(PolymorphicDictionarySerializer<string, ProfileSection>))]
        public Dictionary<string, ProfileSection> Status { get; set; } = new();
        public HashSet<Alert> Alerts => new(this.Status.SelectMany(x => x.Value.Alerts));
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, Model>
    {
        private readonly IClock clock;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly IMapper mapper;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IKeycloakAdministrationClient keycloakClient,
            IMapper mapper,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.keycloakClient = keycloakClient;
            this.mapper = mapper;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<Model> HandleAsync(Command command)
        {
            var data = await this.context.Parties
                .Where(party => party.Id == command.Id)
                .ProjectTo<ProfileData>(this.mapper.ConfigurationProvider)
                .SingleAsync();

            if (data.CollegeLicenceDeclared
                && data.Birthdate != null
                && string.IsNullOrWhiteSpace(data.Cpn))
            {
                // Cert has been entered but no CPN found, likely due to a transient error or delay in PLR record updates. Retry once.
                var newCpn = await this.plrClient.FindCpnAsync(data.LicenceDeclaration.CollegeCode!.Value, data.LicenceDeclaration.LicenceNumber!, data.Birthdate.Value);
                if (newCpn != null)
                {
                    var party = await this.context.Parties
                        .Include(party => party.Credentials)
                        .SingleAsync(party => party.Id == command.Id);
                    party.Cpn = newCpn;
                    await this.keycloakClient.UpdateUserCpn(party.PrimaryUserId, newCpn);
                    if (await this.keycloakClient.AssignClientRole(party.PrimaryUserId, MohClients.LicenceStatus.ClientId, MohClients.LicenceStatus.PractitionerRole))
                    {
                        this.context.BusinessEvents.Add(LicenceStatusRoleAssigned.Create(party.Id, MohClients.LicenceStatus.PractitionerRole, this.clock.GetCurrentInstant()));
                    };
                    await this.context.SaveChangesAsync();
                }

                data.Cpn = newCpn;
            }

            await data.Finalize(this.context, this.plrClient, command.User);

            var profileStatus = new Model
            {
                Status = new List<ProfileSection>
                {
                    ProfileSection.Create<DashboardInfoSection>(data),
                    ProfileSection.Create<AccessAdministratorSection>(data),
                    ProfileSection.Create<BCProviderSection>(data),
                    ProfileSection.Create<CollegeCertificationSection>(data),
                    ProfileSection.Create<DemographicsSection>(data),
                    ProfileSection.Create<EndorsementsSection>(data),
                    ProfileSection.Create<OrganizationDetailsSection>(data),
                    ProfileSection.Create<DriverFitnessSection>(data),
                    ProfileSection.Create<HcimAccountTransferSection>(data),
                    ProfileSection.Create<HcimEnrolmentSection>(data),
                    ProfileSection.Create<MSTeamsSection>(data),
                    ProfileSection.Create<PrescriptionRefillEformsSection>(data),
                    ProfileSection.Create<ProviderReportingPortalSection>(data),
                    ProfileSection.Create<SAEformsSection>(data)
                }
                .ToDictionary(section => section.SectionName, section => section)
            };

            return profileStatus;
        }
    }

    public class ProfileData
    {
        public class LicenceDeclarationDto
        {
            public CollegeCode? CollegeCode { get; set; }
            public string? LicenceNumber { get; set; }

            [MemberNotNullWhen(false, nameof(CollegeCode), nameof(LicenceNumber))]
            public bool HasNoLicence => this.CollegeCode == null || this.LicenceNumber == null;
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public LocalDate? Birthdate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Cpn { get; set; }
        public bool HasBCProviderCredential { get; set; }
        public LicenceDeclarationDto? LicenceDeclaration { get; set; }
        public string? AccessAdministratorEmail { get; set; }
        public bool OrganizationDetailEntered { get; set; }
        public IEnumerable<AccessTypeCode> CompletedEnrolments { get; set; } = Enumerable.Empty<AccessTypeCode>();

        private string? userIdentityProvider;
        public PlrStandingsDigest PartyPlrStanding { get; set; } = default!;
        public PlrStandingsDigest EndorsementPlrStanding { get; set; } = default!;

        [MemberNotNullWhen(true, nameof(Email), nameof(Phone))]
        public bool DemographicsComplete => this.Email != null && this.Phone != null;
        [MemberNotNullWhen(true, nameof(LicenceDeclaration))]
        public bool LicenceDeclarationComplete => this.LicenceDeclaration != null;
        [MemberNotNullWhen(true, nameof(LicenceDeclaration))]
        public bool CollegeLicenceDeclared => this.LicenceDeclaration?.HasNoLicence == false;
        public bool UserIsBCProvider => this.userIdentityProvider == IdentityProviders.BCProvider;
        public bool UserIsHighAssuranceIdentity => this.userIdentityProvider is IdentityProviders.BCServicesCard or IdentityProviders.BCProvider;
        public bool UserIsPhsa => this.userIdentityProvider == IdentityProviders.Phsa;

        public async Task Finalize(
            PidpDbContext context,
            IPlrClient plrClient,
            ClaimsPrincipal? user)
        {
            this.userIdentityProvider = user.GetIdentityProvider();
            this.PartyPlrStanding = await plrClient.GetStandingsDigestAsync(this.Cpn);

            var endorsementCpns = await context.Endorsements
                .Where(endorsement => endorsement.Active
                    && endorsement.EndorsementRelationships.Any(relationship => relationship.PartyId == this.Id))
                .SelectMany(endorsement => endorsement.EndorsementRelationships)
                .Where(relationship => relationship.PartyId != this.Id)
                .Select(relationship => relationship.Party!.Cpn)
                .ToArrayAsync();
            this.EndorsementPlrStanding = await plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);
        }
    }
}
