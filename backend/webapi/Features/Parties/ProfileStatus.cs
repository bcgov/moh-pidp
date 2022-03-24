namespace Pidp.Features.Parties;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Infrastructure;
using Pidp.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using System.Security.Claims;
using NodaTime;
using Pidp.Models.Lookups;

public partial class ProfileStatus
{
    public class Command : ICommand<IDomainResult<Model>>
    {
        public int Id { get; set; }
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

    public class CommandHandler : ICommandHandler<Command, IDomainResult<Model>>
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

        public async Task<IDomainResult<Model>> HandleAsync(Command command)
        {
            var profile = await this.context.Parties
               .Where(party => party.Id == command.Id)
               .ProjectTo<ProfileStatusDto>(this.mapper.ConfigurationProvider)
               .SingleAsync();

            if (profile.Ipc == null
                && profile.CollegeCode != null
                && profile.LicenceNumber != null)
            {
                // Cert has been entered but no IPC found, likely due to a transient error or delay in PLR record updates. Retry once.
                profile.Ipc = await this.RecheckIpc(command.Id, profile.CollegeCode.Value, profile.LicenceNumber, profile.Birthdate);
            }

            profile.PlrRecordStatus = await this.client.GetRecordStatus(profile.Ipc);
            //profile.User = ??

            var profileStatus = new Model
            {
                Status = new List<Model.ProfileSection>
                {
                    new Model.Demographics(profile),
                    new Model.CollegeCertification(profile),
                    new Model.SAEforms(profile),
                    new Model.HcimReEnrolment(profile)
                }
                .ToDictionary(section => section.SectionName, section => section)
            };

            return DomainResult.Success(profileStatus);
        }

        private async Task<string?> RecheckIpc(int partyId, CollegeCode collegeCode, string licenceNumber, LocalDate birthdate)
        {
            var newIpc = await this.client.GetPlrRecord(collegeCode, licenceNumber, birthdate);
            if (newIpc != null)
            {
                var cert = await this.context.PartyCertifications
                    .SingleAsync(cert => cert.PartyId == partyId);
                cert.Ipc = newIpc;
                await this.context.SaveChangesAsync();
            }

            return newIpc;
        }
    }

    public class ProfileStatusDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public LocalDate Birthdate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public CollegeCode? CollegeCode { get; set; }
        public string? LicenceNumber { get; set; }
        public string? Ipc { get; set; }
        public IEnumerable<AccessType> CompletedEnrolments { get; set; } = Enumerable.Empty<AccessType>();

        // Computed after projection
        public PlrRecordStatus? PlrRecordStatus { get; set; }
        public ClaimsPrincipal? User { get; set; }

        public bool DemographicsEntered => this.Email != null && this.Phone != null;
        public bool CollegeCertificationEntered => this.CollegeCode.HasValue && this.LicenceNumber != null;
    }
}
