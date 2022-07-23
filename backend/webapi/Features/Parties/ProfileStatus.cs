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
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
using static Pidp.Features.Parties.ProfileStatus.ProfileStatusDto;

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

        public abstract class ProfileSection
        {
            internal abstract string SectionName { get; }
            public HashSet<Alert> Alerts { get; set; } = new();
            public StatusCode StatusCode { get; set; }

            public bool IsComplete => this.StatusCode == StatusCode.Complete;

            public ProfileSection(ProfileStatusDto profile) => this.SetAlertsAndStatus(profile);

            protected abstract void SetAlertsAndStatus(ProfileStatusDto profile);
        }

        public enum Alert
        {
            TransientError = 1,
            PlrBadStanding
        }

        public enum StatusCode
        {
            Incomplete = 1,
            Complete,
            Locked,
            Error,
            Hidden
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, Model>
    {
        private readonly IMapper mapper;
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public CommandHandler(
            IMapper mapper,
            IPlrClient client,
            PidpDbContext context)
        {
            this.mapper = mapper;
            this.client = client;
            this.context = context;
        }

        public async Task<Model> HandleAsync(Command command)
        {
            var profile = await this.context.Parties
               .Where(party => party.Id == command.Id)
               .ProjectTo<ProfileStatusDto>(this.mapper.ConfigurationProvider)
               .SingleAsync();

            if (profile.LicenceDeclared
                && profile.Cpn == null)
            {
                // Cert has been entered but no CPN found, likely due to a transient error or delay in PLR record updates. Retry once.
                profile.Cpn = await this.RecheckCpn(command.Id, profile.LicenceDeclaration, profile.Birthdate);
            }

            profile.PlrGoodStanding = await this.client.IsGoodStanding(profile.Cpn);
            profile.User = command.User;

            var profileStatus = new Model
            {
                Status = new List<Model.ProfileSection>
                {
                    new Model.Demographics(profile),
                    new Model.CollegeCertification(profile),
                    new Model.AccessAdministrator(profile),
                    new Model.OrganizationDetails(profile),
                    new Model.DriverFitness(profile),
                    new Model.SAEforms(profile),
                    new Model.HcimAccountTransfer(profile),
                    new Model.HcimEnrolment(profile),
                    new Model.Uci(profile)
                }
                .ToDictionary(section => section.SectionName, section => section)
            };

            return profileStatus;
        }

        private async Task<string?> RecheckCpn(int partyId, LicenceDeclarationDto declaration, LocalDate? birthdate)
        {
            if (declaration.NoLicence
                || birthdate == null)
            {
                return null;
            }

            var newCpn = await this.client.FindCpn(declaration.CollegeCode.Value, declaration.LicenceNumber, birthdate.Value);
            if (newCpn != null)
            {
                var party = await this.context.Parties
                    .SingleAsync(party => party.Id == partyId);
                party.Cpn = newCpn;
                await this.context.SaveChangesAsync();
            }

            return newCpn;
        }
    }

    public class ProfileStatusDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public LocalDate? Birthdate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Cpn { get; set; }
        public LicenceDeclarationDto? LicenceDeclaration { get; set; }
        public string? AccessAdministratorEmail { get; set; }
        public bool OrganizationDetailEntered { get; set; }
        public IEnumerable<AccessTypeCode> CompletedEnrolments { get; set; } = Enumerable.Empty<AccessTypeCode>();

        // Resolved after projection
        public bool? PlrGoodStanding { get; set; }
        public ClaimsPrincipal? User { get; set; }

        // Computed Properties
        [MemberNotNullWhen(true, nameof(Email), nameof(Phone))]
        public bool DemographicsEntered => this.Email != null && this.Phone != null;
        [MemberNotNullWhen(true, nameof(LicenceDeclaration))]
        public bool LicenceDeclared => this.LicenceDeclaration?.NoLicence == false;
        public bool UserIsBcServicesCard => this.User.GetIdentityProvider() == ClaimValues.BCServicesCard;
        public bool UserIsPhsa => this.User.GetIdentityProvider() == ClaimValues.Phsa;

        public class LicenceDeclarationDto
        {
            public CollegeCode? CollegeCode { get; set; }
            public string? LicenceNumber { get; set; }

            [MemberNotNullWhen(false, nameof(CollegeCode), nameof(LicenceNumber))]
            public bool NoLicence => this.CollegeCode == null || this.LicenceNumber == null;
        }
    }
}
