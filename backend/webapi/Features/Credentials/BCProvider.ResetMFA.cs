namespace Pidp.Features.Credentials;

using System.Text.Json.Serialization;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;

public class BCProviderResetMFA
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
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

            if (await this.client.RemoveAuthMethods(bcProviderId))
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
