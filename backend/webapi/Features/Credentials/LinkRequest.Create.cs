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

public class LinkRequestCreate
{
    public class Command : ICommand<IDomainResult<Guid>>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string Idp { get; set; } = string.Empty;

        [JsonIgnore]
        public ClaimsPrincipal? User { get; set; }

        public Command WithUser(ClaimsPrincipal user)
        {
            this.User = user;
            return this;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;


            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Idp).NotEmpty().Equal(IdentityProviders.BCProvider); // Currently, we only support linking from BCSC to BC Provider.
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<Guid>>
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

        public async Task<IDomainResult<Guid>> HandleAsync(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
