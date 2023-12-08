namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Features.AccessRequests;
using static Pidp.Features.Parties.ProfileStatus.Model;
using Pidp.Infrastructure;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public partial class ProfileStatus
{
    public class Query : IQuery<Model>
    {
        public int Id { get; set; }
        [JsonIgnore]
        public ClaimsPrincipal? User { get; set; }

        public Query WithUser(ClaimsPrincipal user)
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

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, Model>
    {
        private readonly IMapper mapper;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public QueryHandler(
            IMapper mapper,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.mapper = mapper;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<Model> HandleAsync(Query query)
        {
            var data = await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<ProfileData>(this.mapper.ConfigurationProvider)
                .SingleAsync();

            await data.Finalize(this.context, this.plrClient, query.User);

            return new Model
            {
                Status = new List<ProfileSection>
                {
                    ProfileSection.Create<DashboardInfoSection>(data),
                    ProfileSection.Create<AccessAdministratorSection>(data),
                    ProfileSection.Create<BCProviderSection>(data),
                    ProfileSection.Create<CollegeCertificationSection>(data),
                    ProfileSection.Create<DemographicsSection>(data),
                    ProfileSection.Create<EndorsementsSection>(data),
                    ProfileSection.Create<UserAccessAgreementSection>(data),
                    ProfileSection.Create<OrganizationDetailsSection>(data),
                    ProfileSection.Create<DriverFitnessSection>(data),
                    ProfileSection.Create<EdrdEformsSection>(data),
                    ProfileSection.Create<HcimAccountTransferSection>(data),
                    ProfileSection.Create<HcimEnrolmentSection>(data),
                    ProfileSection.Create<ImmsBCEformsSection>(data),
                    ProfileSection.Create<MSTeamsClinicMemberSection>(data),
                    ProfileSection.Create<MSTeamsPrivacyOfficerSection>(data),
                    ProfileSection.Create<PrescriptionRefillEformsSection>(data),
                    ProfileSection.Create<ProviderReportingPortalSection>(data),
                    ProfileSection.Create<PrimaryCareRosteringSection>(data),
                    ProfileSection.Create<SAEformsSection>(data)
                }
                .ToDictionary(section => section.SectionName, section => section)
            };
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

        // Mapped
        public int Id { get; set; }
        public string DisplayFullName { get; set; } = string.Empty;
        public LocalDate? Birthdate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Cpn { get; set; }
        public bool HasBCProviderCredential { get; set; }
        public bool HasPendingEndorsementRequest { get; set; }
        public LicenceDeclarationDto? LicenceDeclaration { get; set; }
        public string? AccessAdministratorEmail { get; set; }
        public bool OrganizationDetailEntered { get; set; }
        public IEnumerable<AccessTypeCode> CompletedEnrolments { get; set; } = Enumerable.Empty<AccessTypeCode>();

        // Computed in Finalize()
        private string? userIdentityProvider;
        public PlrStandingsDigest EndorsementPlrStanding { get; set; } = default!;
        public bool HasMSTeamsClinicEndorsement { get; set; }
        public bool HasPrpAuthorizedLicence { get; set; }
        public PlrStandingsDigest PartyPlrStanding { get; set; } = default!;

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

            var possiblePrpLicenceNumbers = this.PartyPlrStanding
                .With(ProviderReportingPortal.AllowedIdentifierTypes)
                .LicenceNumbers;
            if (this.UserIsBCProvider && possiblePrpLicenceNumbers.Any())
            {
                this.HasPrpAuthorizedLicence = await context.PrpAuthorizedLicences
                    .AnyAsync(authorizedLicence => possiblePrpLicenceNumbers.Contains(authorizedLicence.LicenceNumber));
            }

            var endorsementDtos = await context.ActiveEndorsementRelationships(this.Id)
                .Select(relationship => new
                {
                    relationship.Party!.Cpn,
                    IsMSTeamsPrivacyOfficer = context.MSTeamsClinics.Any(clinic => clinic.PrivacyOfficerId == relationship.PartyId)
                })
                .ToArrayAsync();

            this.HasMSTeamsClinicEndorsement = endorsementDtos.Any(dto => dto.IsMSTeamsPrivacyOfficer);
            // We should defer this check if possible. See DriverFitnessSection.
            this.EndorsementPlrStanding = await plrClient.GetAggregateStandingsDigestAsync(endorsementDtos.Select(dto => dto.Cpn));

            this.HasPendingEndorsementRequest = await context.EndorsementRequests
                .Where(request => (request.ReceivingPartyId == this.Id && request.Status == EndorsementRequestStatus.Received)
                    || (request.RequestingPartyId == this.Id && request.Status == EndorsementRequestStatus.Approved))
                .AnyAsync();
        }
    }
}
