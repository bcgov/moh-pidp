namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
// using Pidp.Infrastructure.Auth;
// using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class DriverFitness
{
    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        // private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            // IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            // this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.DriverFitness),
                    party.Cpn,
                    party.Email
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !await this.plrClient.GetStandingAsync(dto.Cpn))
            {
                this.logger.LogDriverFitnessAccessRequestDenied();
                return DomainResult.Failed();
            }

            // TODO assign role
            // if (!await this.keycloakClient.AssignClientRole(dto.UserId, ?, ?))
            // {
            //     return DomainResult.Failed();
            // }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.DriverFitness,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            await this.SendConfirmationEmailAsync(dto.Email);

            return DomainResult.Success();
        }

        private async Task SendConfirmationEmailAsync(string partyEmail)
        {
            // TODO email text
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "Driver Fitness Enrolment Confirmation",
                body: $"Driver Fitness Enrolment Confirmation"
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class DriverFitnessLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Driver Fitness Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogDriverFitnessAccessRequestDenied(this ILogger logger);
}
