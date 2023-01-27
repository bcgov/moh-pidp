namespace Pidp.Features.Bcp;

using FluentValidation;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;

using Microsoft.Graph;
using Azure.Identity;

public class Create
{
    public class Command : ICommand<string>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;

            this.RuleFor(x => x.FirstName).NotEmpty().MatchesUserClaim(user, Claims.GivenName);
            this.RuleFor(x => x.LastName).NotEmpty().MatchesUserClaim(user, Claims.FamilyName);
        }
    }

    public class CommandHandler : ICommandHandler<Command, string>
    {

        public CommandHandler() { }

        public async Task<string> HandleAsync(Command command)
        {
            // This code depends on packages:
            // - Microsoft.Identity.Client
            //   (this requiremnent should be confirmed when creating the non-POC implementation; I think Azure.Identity is coming from this package)
            // - Microsoft.Identity.Web.MicrosoftGraph

            // NOTE: For this spike, both User.ReadWrite.All and Directory.ReadWrite.All API permissions
            //       were granted with admin consent on the app registration.
            //       The Graph method Users.Create (AddAsync() below) is supposed to work with EITHER and not necessarily 
            //       both permissions, but this was not tested for this spike.

            // Client ID from Azure App Registration overview
            var clientId = "3ae76269-7f47-4613-a372-6e84b00bab55";
            // Tenant ID from Azure App Registration overview
            var tenantId = "9a67b082-821d-4512-ba62-b2bf5f3c9664"; // "common"
            // Client Secret Value created in Azure App Registration configuration.
            // NOTE: This is the Value and not the Name of the secret.
            // NOTE: Consider using certificate instead of secret in final implementation
            var clientSecret = "GET FROM AZURE APP CONFIG";

            //var scopes = new[] { "User.ReadWrite.All/.default" };//, "Directory.ReadWrite.All" };
            var scopes = new string[] { "https://graph.microsoft.com/.default" };
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
            var client = new GraphServiceClient(credential, scopes);

            // NOTE: This must match an allowed domain as configured in Azure or AddAsync() below will fail.
            var userprincipal = $"{command.FirstName}.{command.LastName}@bcproviderlab.ca";

            // NOTE: These is the minimum set of properties that must be set for the user creation to work.
            var user = new User()
            {
                AccountEnabled = true,
                DisplayName = command.FirstName + " " + command.LastName,
                MailNickname = command.FirstName,
                UserPrincipalName = userprincipal,
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = "Graph-Spike"
                }
            };

            var result = await client.Users.Request().AddAsync(user);
            return result.UserPrincipalName;
        }
    }
}
