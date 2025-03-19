namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Microsoft.Graph.Models;

public class BCProviderInvite
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string UserPrincipalName { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.UserPrincipalName).NotEmpty();
        }
    }

    public class CommandHandler(IBCProviderClient client, PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient client = client;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var createdGuestId = await this.client.SendInvite(command.UserPrincipalName);
            if (createdGuestId == null)
            {
                Console.WriteLine("Failed to send BCProvider invite");
                return DomainResult.Failed();
            }

            var user = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new User
                {
                    GivenName = party.FirstName,
                    Surname = party.LastName
                })
                .SingleAsync();

            if (await this.client.UpdateUser(createdGuestId, user))
            {
                if (await this.client.AssignAccessPackage(createdGuestId))
                {
                    return DomainResult.Success();
                }
                else
                {
                    Console.WriteLine("Failed to assign access package");
                    return DomainResult.Failed();
                }
            }
            else
            {
                Console.WriteLine("Failed to update BCProvider user's first/last name");
                return DomainResult.Failed();
            }
        }
    }
}
