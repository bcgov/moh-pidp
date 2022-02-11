namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Features.Parties.ProfileStatusInternal;
using Pidp.Infrastructure;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class ProfileStatus
{
    public class Command : ICommand<IDomainResult<Model>>
    {
        public int Id { get; set; }
    }

    public class Model
    {
        public List<string> Alerts { get; set; } = new();
        [JsonConverter(typeof(PolymorphicDictionarySerializer<string, ProfileSection>))]
        public Dictionary<string, ProfileSection> Status { get; set; } = new();

        public static class Section
        {
            public const string Demographics = "demographics";
            public const string CollegeCertification = "collegeCertification";
            public const string SAEforms = "saEforms";
        }

        public class ProfileSection
        {
            public StatusCode StatusCode { get; set; }
        }

        public enum StatusCode
        {
            Incomplete = 1,
            Complete,
            Locked,
            Error
        }

        public bool SectionIsComplete(string sectionName)
        {
            return this.Status.TryGetValue(sectionName, out var section)
                && section.StatusCode == StatusCode.Complete;
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
                .ProjectTo<ProfileDto>(this.mapper.ConfigurationProvider)
                .SingleAsync();

            if (profile.Ipc == null
                && profile.CollegeCode != null
                && profile.LicenceNumber != null)
            {
                // Cert has been entered but no IPC found, likely due to a transient error or delay in PLR record updates. Retry once.
                profile.Ipc = await this.RecheckIpc(command.Id, profile.CollegeCode.Value, profile.LicenceNumber, profile.Birthdate);
            }

            profile.PlrRecordStatus = await this.client.GetRecordStatus(profile.Ipc);

            var resolvers = new IProfileSectionResolver[]
            {
                new DemographicsSectionResolver(),
                new CollegeCertificationSectionResolver(),
                new SAEformsSectionResolver()
            };

            var profileStatus = new Model();
            foreach (var resolver in resolvers)
            {
                resolver.ComputeSection(profileStatus, profile);
            }

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

        public class ProfileDto
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public LocalDate Birthdate { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public CollegeCode? CollegeCode { get; set; }
            public string? LicenceNumber { get; set; }
            public string? Ipc { get; set; }
            // For Work Setting, currently unused
            // public string? JobTitle { get; set; }
            // public string? FacilityName { get; set; }
            // public string? FacilityStreet { get; set; }
            public IEnumerable<AccessType> CompletedEnrolments { get; set; } = Enumerable.Empty<AccessType>();

            // Computed after projection
            public PlrRecordStatus? PlrRecordStatus { get; set; }
        }
    }
}
