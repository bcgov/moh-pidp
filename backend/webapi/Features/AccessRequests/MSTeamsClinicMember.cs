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
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class MSTeamsClinicMember
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public int ClinicId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.ClinicId).GreaterThan(0);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly ILogger<CommandHandler> logger;
        private readonly IMapper mapper;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            ILogger<CommandHandler> logger,
            IMapper mapper,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            this.logger = logger;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var enrolmentDto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .ProjectTo<EnrolmentDto>(this.mapper.ConfigurationProvider)
                .SingleAsync();

            var clinicDto = await this.context.MSTeamsClinics
                .Where(clinic => clinic.Id == command.ClinicId
                    && this.context.ActiveEndorsementRelationships(command.PartyId)
                        .Any(relationship => relationship.PartyId == clinic.PrivacyOfficerId))
                .ProjectTo<ClinicDto>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (enrolmentDto.AlreadyEnroled
                || enrolmentDto.Email == null
                || clinicDto == null)
            {
                this.logger.LogAccessRequestDenied();
                return DomainResult.Failed();
            }

            this.context.MSTeamsClinicMemberEnrolments.Add(new MSTeamsClinicMemberEnrolment
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.MSTeamsClinicMember,
                RequestedOn = this.clock.GetCurrentInstant(),
                ClinicId = command.ClinicId
            });

            await this.context.SaveChangesAsync();

            await this.SendEnrolmentEmailAsync(enrolmentDto, clinicDto);
            await this.SendPrivacyOfficerConfirmationEmailAsync(clinicDto.PrivacyOfficerEmail!, enrolmentDto.Name);

            return DomainResult.Success();
        }

        private async Task SendEnrolmentEmailAsync(EnrolmentDto enrolment, ClinicDto clinic)
        {
            var body = new EnrolmentEmailModel(enrolment, clinic, this.clock.GetCurrentInstant());
            var email = new Email(
                from: EmailService.PidpEmail,
                to: "enrolment_securemessaging@fraserhealth.ca",
                subject: $"Add User to MS Teams (Privacy Officer: {clinic.PrivacyOfficerName})",
                body: $"<pre>{JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true })}</pre>"
            );

            await this.emailService.SendAsync(email);
        }

        private async Task SendPrivacyOfficerConfirmationEmailAsync(string privacyOfficerEmail, string enrolleeName)
        {
            var email = new Email(
                from: EmailService.PidpEmail,
                to: privacyOfficerEmail,
                subject: "MS Teams for Clinical Use Member Enrolment Confirmation",
                body: $"Team member {enrolleeName} has been requested to be added to your clinic's MS Teams group."
            );
            await this.emailService.SendAsync(email);
        }

        public class EnrolmentDto
        {
            public LocalDate? Birthdate { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public bool AlreadyEnroled { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class ClinicDto
        {
            public string PrivacyOfficerName { get; set; } = string.Empty;
            public string? PrivacyOfficerEmail { get; set; }
            public string Name { get; set; } = string.Empty;
            public ClinicAddressDto Address { get; set; } = new();

            public class ClinicAddressDto
            {
                public string CountryCode { get; set; } = string.Empty;
                public string ProvinceCode { get; set; } = string.Empty;
                public string Street { get; set; } = string.Empty;
                public string City { get; set; } = string.Empty;
                public string Postal { get; set; } = string.Empty;
            }
        }

        private class EnrolmentEmailModel
        {
            public string EnrolmentDate { get; set; }
            public string MemberName { get; set; }
            public string? MemberBirthdate { get; set; }
            public string? MemberEmail { get; set; }
            public string? MemberPhone { get; set; }
            public string PrivacyOfficerName { get; set; }
            public string ClinicName { get; set; }
            public Address ClinicAddress { get; set; }

            public class Address
            {
                public string CountryCode { get; set; } = string.Empty;
                public string ProvinceCode { get; set; } = string.Empty;
                public string Street { get; set; } = string.Empty;
                public string City { get; set; } = string.Empty;
                public string Postal { get; set; } = string.Empty;
            }

            public EnrolmentEmailModel(EnrolmentDto enrolment, ClinicDto clinic, Instant enrolmentDate)
            {
                this.EnrolmentDate = enrolmentDate.InZone(DateTimeZoneProviders.Tzdb.GetZoneOrNull("America/Vancouver")!).Date.ToString();
                this.MemberName = enrolment.Name;
                this.MemberBirthdate = enrolment.Birthdate?.ToString();
                this.MemberEmail = enrolment.Email;
                this.MemberPhone = enrolment.Phone;
                this.PrivacyOfficerName = clinic.PrivacyOfficerName;
                this.ClinicName = clinic.Name;
                this.ClinicAddress = new Address
                {
                    CountryCode = clinic.Address!.CountryCode,
                    ProvinceCode = clinic.Address.ProvinceCode,
                    Street = clinic.Address.Street,
                    City = clinic.Address.City,
                    Postal = clinic.Address.Postal
                };
            }
        }
    }
}

public static partial class MSTeamsClinicMemberLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "MS Teams Clinic Member Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<MSTeamsClinicMember.CommandHandler> logger);
}
