namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Security.Claims;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class LinkRequestComplete
{
    public class Command : ICommand<IDomainResult<int>>
    {
        public int PartyId { get; set; }
        public Guid CredentialLinkRequestId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;

            this.RuleFor(x => x.PartyId).GreaterThan(0);
            // TODO
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<int>>
    {
        private readonly IBCProviderClient client;
        private readonly ILogger logger;
        private readonly PidpDbContext context;

        public CommandHandler(
            IBCProviderClient client,
            ILogger<CommandHandler> logger,
            PidpDbContext context)
        {
            this.client = client;
            this.logger = logger;
            this.context = context;
        }

        public async Task<IDomainResult<int>> HandleAsync(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
