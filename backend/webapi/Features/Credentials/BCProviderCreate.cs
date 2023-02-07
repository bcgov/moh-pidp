namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models;

public class BCProviderCreate
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
            this.RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient client;
        private readonly PidpDbContext context;

        public CommandHandler(IBCProviderClient client, PidpDbContext context)
        {
            this.client = client;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            if (await this.context.Credentials
                .AnyAsync(credential => credential.PartyId == command.PartyId
                    && credential.IdentityProvider == IdentityProviders.BCProvider))
            {
                // Already has a BC Provider Credential
                return DomainResult.Failed();
            }

            var createdUser = await this.client.CreateBCProviderAccount(new UserRepresentation
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Password = command.Password
            });

            if (createdUser == null)
            {
                return DomainResult.Failed();
            }

            this.context.Credentials.Add(new Credential
            {
                PartyId = command.PartyId,
                IdpId = createdUser.UserPrincipalName,
                IdentityProvider = IdentityProviders.BCProvider
            });

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}
