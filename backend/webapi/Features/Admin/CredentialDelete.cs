namespace Pidp.Features.Admin;

using DomainResults.Common;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;

public class CredentialDelete
{
    public class Command : ICommand<IDomainResult>
    {
        public required int PartyId { get; set; }
        public required int CredentialId { get; set; }
        public required bool DeleteFromBcProvider { get; set; }
    }

    public class CommandHandler(
        IBCProviderClient bcProviderClient,
        PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient bcProviderClient = bcProviderClient;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var credential = await this.context.Credentials
                .FirstOrDefaultAsync(c => c.PartyId == command.PartyId && c.Id == command.CredentialId);

            if (credential == null)
            {
                return DomainResult.Failed();
            }

            if (command.DeleteFromBcProvider && credential.IdentityProvider == IdentityProviders.BCProvider && !string.IsNullOrEmpty(credential.IdpId))
            {
                var success = await this.bcProviderClient.DeleteBCProviderAccount(credential.IdpId);
                if (!success)
                {
                    return DomainResult.Failed("Failed to delete account from BC Provider Active Directory.");
                }
            }

            this.context.Credentials.Remove(credential);
            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}
