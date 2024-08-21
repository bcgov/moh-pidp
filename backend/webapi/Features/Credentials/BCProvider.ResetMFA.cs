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

    public class CommandHandler(
        IBCProviderClient client,
        PidpDbContext context,
        ILogger<CommandHandler> logger) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient client = client;
        private readonly PidpDbContext context = context;
        private readonly ILogger<CommandHandler> logger = logger;

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
                this.logger.LogResetMFA(command.PartyId, bcProviderId);
                return DomainResult.Success();
            }
            else
            {
                this.logger.LogResetMFAError(command.PartyId, bcProviderId);
                return DomainResult.Failed();
            }
        }
    }
}

public static partial class BCProviderResetMFALoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Reset BCProvider MFA for party {PartyId} with User Principal Name {bcProviderId}.")]
    public static partial void LogResetMFA(this ILogger logger, int partyId, string bcProviderId);

    [LoggerMessage(2, LogLevel.Error, "Error when attempting to reset BCProvider MFA for party {PartyId} with User Principal Name {bcProviderId}.")]
    public static partial void LogResetMFAError(this ILogger logger, int partyId, string bcProviderId);
}
