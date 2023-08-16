namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Security.Claims;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models;
using Pidp.Models.Lookups;

public class Create
{
    public class Command : ICommand<IDomainResult<int>>
    {
        public int PartyId { get; set; }

        [JsonIgnore]
        public Guid CredentialLinkToken { get; set; }
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; } = new();
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<int>>
    {
        private readonly ILogger logger;
        private readonly PidpDbContext context;

        public CommandHandler(
            ILogger<CommandHandler> logger,
            PidpDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<IDomainResult<int>> HandleAsync(Command command)
        {
            // TODO:
            // Retrieve Ticket by GUID Token
            // Validate Ticket (Party ID, expiry, idp)
            // Create Credential from User
            // Set Ticket Claimed = true
            // Create + Raise Domain Event to update ADD
            // Return Party ID
            // Update AAD in Domain Event
            return DomainResult.Success(1001);
        }
    }
}
