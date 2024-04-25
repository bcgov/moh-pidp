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
    public static IdentifierType[] AllowedIdentifierTypes => new[] { IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse };

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

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly ILogger<CommandHandler> logger;
        private readonly IMapper mapper;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            ILogger<CommandHandler> logger,
            IMapper mapper,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            this.logger = logger;
            this.mapper = mapper;
            this.plrClient = plrClient;
            this.context = context;
        }

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
                }).ToList() ?? new()
            );

            var email = new Email(
                from: EmailService.PidpEmail,
                to: "enrolment_securemessaging@fraserhealth.ca",
                subject: "New MS Teams for Clinical Use Enrolment",
                body: $"<pre>{JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true })}</pre>"
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

        private class EnrolmentEmailModel
        {
            public string EnrolmentDate { get; set; }
            public string PrivacyOfficerName { get; set; }
            public string? PrivacyOfficerBirthdate { get; set; }
            public string? PrivacyOfficerEmail { get; set; }
            public string? PrivacyOfficerPhone { get; set; }
            public List<PlrRecord> PrivacyOfficerPlrRecords { get; set; }
            public string ClinicName { get; set; }
            public Command.Address ClinicAddress { get; set; }

            public class PlrRecord
            {
                public string? CollegeId { get; set; }
                public string? IdentifierType { get; set; }
                public string? ProviderRoleType { get; set; }
                public string? StatusCode { get; set; }
                public string? StatusReasonCode { get; set; }
            }

            public EnrolmentEmailModel(EnrolmentDto enrolmentDto, Command command, Instant enrolmentDate, List<PlrRecord> plrRecords)
            {
                this.EnrolmentDate = enrolmentDate.InZone(DateTimeZoneProviders.Tzdb.GetZoneOrNull("America/Vancouver")!).Date.ToString();
                this.PrivacyOfficerName = enrolmentDto.FullName;
                this.PrivacyOfficerBirthdate = enrolmentDto.Birthdate?.ToString();
                this.PrivacyOfficerEmail = enrolmentDto.Email;
                this.PrivacyOfficerPhone = enrolmentDto.Phone;
                this.PrivacyOfficerPlrRecords = plrRecords;
                this.ClinicName = command.ClinicName;
                this.ClinicAddress = command.ClinicAddress;
            }
        }
    }
}

public static partial class MSTeamsPrivacyOfficerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "MS Teams Privacy Officer Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<MSTeamsPrivacyOfficer.CommandHandler> logger);
}
