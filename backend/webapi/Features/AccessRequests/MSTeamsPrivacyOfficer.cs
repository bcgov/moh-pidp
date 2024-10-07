namespace Pidp.Features.AccessRequests;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class MSTeamsPrivacyOfficer
{
    public static IdentifierType[] AllowedIdentifierTypes => [IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse];

    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public Address ClinicAddress { get; set; } = new();

        public class Address
        {
            public string CountryCode { get; set; } = string.Empty;
            public string ProvinceCode { get; set; } = string.Empty;
            public string Street { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Postal { get; set; } = string.Empty;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.ClinicName).NotEmpty();

            this.RuleFor(x => x.ClinicAddress).NotNull();
            this.RuleFor(x => x.ClinicAddress.CountryCode).NotEmpty();
            this.RuleFor(x => x.ClinicAddress.ProvinceCode).NotEmpty();
            this.RuleFor(x => x.ClinicAddress.Street).NotEmpty();
            this.RuleFor(x => x.ClinicAddress.City).NotEmpty();
            this.RuleFor(x => x.ClinicAddress.Postal).NotEmpty();
        }
    }

    public class CommandHandler(
        IClock clock,
        IEmailService emailService,
        ILogger<CommandHandler> logger,
        IMapper mapper,
        IPlrClient plrClient,
        PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock = clock;
        private readonly IEmailService emailService = emailService;
        private readonly ILogger<CommandHandler> logger = logger;
        private readonly IMapper mapper = mapper;
        private readonly IPlrClient plrClient = plrClient;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .ProjectTo<EnrolmentDto>(this.mapper.ConfigurationProvider)
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !(await this.plrClient.GetStandingsDigestAsync(dto.Cpn))
                    .With(AllowedIdentifierTypes)
                    .HasGoodStanding)
            {
                this.logger.LogAccessRequestDenied();
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.MSTeamsPrivacyOfficer,
                RequestedOn = this.clock.GetCurrentInstant(),
            });
            this.context.MSTeamsClinics.Add(new MSTeamsClinic
            {
                PrivacyOfficerId = command.PartyId,
                Name = command.ClinicName,
                Address = this.mapper.Map<MSTeamsClinicAddress>(command.ClinicAddress)
            });

            await this.context.SaveChangesAsync();

            await this.SendEnrolmentEmailAsync(dto, command);
            await this.SendConfirmationEmailAsync(dto.Email, dto.FullName);

            return DomainResult.Success();
        }

        private async Task SendEnrolmentEmailAsync(EnrolmentDto dto, Command command)
        {
            var plrRecords = await this.plrClient.GetRecordsAsync(dto.Cpn);
            var model = new EnrolmentEmailModel
            (
                dto,
                command,
                this.clock.GetCurrentInstant(),
                plrRecords?.Select(record => new EnrolmentEmailModel.PlrRecord
                {
                    CollegeId = record.CollegeId,
                    IdentifierType = record.IdentifierType,
                    ProviderRoleType = record.ProviderRoleType,
                    StatusCode = record.StatusCode,
                    StatusReasonCode = record.StatusReasonCode
                }).ToList() ?? []
            );

            var email = new Email(
                from: EmailService.PidpEmail,
                to: "enrolment_securemessaging@fraserhealth.ca",
                subject: "New MS Teams for Clinical Use Enrolment",
                body: $"<pre>{model.Serialize()}</pre>"
            );
            await this.emailService.SendAsync(email);
        }

        private async Task SendConfirmationEmailAsync(string partyEmail, string partyName)
        {
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "MS Teams for Clinical Use Enrolment Confirmation",
                body: $"Dear {partyName},<br><br>You have been successfully enrolled for MS Teams for Clinical Use.<br>The Fraser Health mHealth team will contact you via email with account information and setup instructions. In the meantime please email <a href=\"mailto:securemessagingsupport@fraserhealth.ca\">securemessagingsupport@fraserhealth.ca</a> if you have any questions."
            );
            await this.emailService.SendAsync(email);
        }

        public class EnrolmentDto
        {
            public string FullName { get; set; } = string.Empty;
            public LocalDate? Birthdate { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public bool AlreadyEnroled { get; set; }
            public string? Cpn { get; set; }
        }

        private sealed class EnrolmentEmailModel(
            EnrolmentDto enrolmentDto,
            Command command,
            Instant enrolmentDate,
            List<EnrolmentEmailModel.PlrRecord> plrRecords)
        {
            public static readonly JsonSerializerOptions SerializationOptions = new() { WriteIndented = true };

            public string EnrolmentDate { get; set; } = enrolmentDate.InZone(DateTimeZoneProviders.Tzdb.GetZoneOrNull("America/Vancouver")!).Date.ToString();
            public string PrivacyOfficerName { get; set; } = enrolmentDto.FullName;
            public string? PrivacyOfficerBirthdate { get; set; } = enrolmentDto.Birthdate?.ToString();
            public string? PrivacyOfficerEmail { get; set; } = enrolmentDto.Email;
            public string? PrivacyOfficerPhone { get; set; } = enrolmentDto.Phone;
            public List<PlrRecord> PrivacyOfficerPlrRecords { get; set; } = plrRecords;
            public string ClinicName { get; set; } = command.ClinicName;
            public Command.Address ClinicAddress { get; set; } = command.ClinicAddress;

            public sealed class PlrRecord
            {
                public string? CollegeId { get; set; }
                public string? IdentifierType { get; set; }
                public string? ProviderRoleType { get; set; }
                public string? StatusCode { get; set; }
                public string? StatusReasonCode { get; set; }
            }

            public string Serialize() => JsonSerializer.Serialize(this, SerializationOptions);
        }
    }
}

public static partial class MSTeamsPrivacyOfficerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "MS Teams Privacy Officer Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<MSTeamsPrivacyOfficer.CommandHandler> logger);
}
