namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models;

public class BCProviderPassword
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.NewPassword).NotEmpty();
        }
    }

    public class CommandHandler(
        IBCProviderClient client,
        PidpDbContext context,
        IClock clock) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient client = client;
        private readonly PidpDbContext context = context;
        private readonly IClock clock = clock;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var userPrincipalName = await this.context.Credentials
                .Where(credential => credential.PartyId == command.PartyId
                    && credential.IdentityProvider == IdentityProviders.BCProvider)
                .Select(credential => credential.IdpId)
                .SingleOrDefaultAsync();

            if (userPrincipalName == null)
            {
                return DomainResult.NotFound();
            }

            if (await this.client.UpdatePassword(userPrincipalName, command.NewPassword))
            {
                this.context.BusinessEvents.Add(BCProviderPasswordReset.Create(command.PartyId, userPrincipalName, this.clock.GetCurrentInstant()));
                await this.context.SaveChangesAsync();
                return DomainResult.Success();
            }
            else
            {
                return DomainResult.Failed();
            }
        }
    }
}
