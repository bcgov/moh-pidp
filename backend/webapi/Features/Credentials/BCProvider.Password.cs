namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;

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

    public class CommandHandler(IBCProviderClient client, PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient client = client;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var bcProviderId = await this.context.Credentials
                .Where(credential => credential.PartyId == command.PartyId
                    && credential.IdentityProvider == IdentityProviders.BCProvider)
                .Select(credential => credential.IdpId)
                .SingleOrDefaultAsync();

            if (bcProviderId == null)
            {
                return DomainResult.NotFound();
            }

            if (await this.client.UpdatePassword(bcProviderId, command.NewPassword))
            {
                return DomainResult.Success();
            }
            else
            {
                return DomainResult.Failed();
            }
        }
    }
}
