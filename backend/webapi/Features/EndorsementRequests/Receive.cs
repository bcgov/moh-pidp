// namespace Pidp.Features.EndorsementRequests;

// using DomainResults.Common;
// using FluentValidation;
// using HybridModelBinding;
// using Microsoft.EntityFrameworkCore;
// using System.Text.Json.Serialization;

// using Pidp.Data;
// using Pidp.Models;

// public class Receive
// {
//     public class Command : ICommand<IDomainResult>
//     {
//         [JsonIgnore]
//         [HybridBindProperty(Source.Route)]
//         public int PartyId { get; set; }
//         public Guid Token { get; set; }
//     }

//     public class CommandValidator : AbstractValidator<Command>
//     {
//         public CommandValidator()
//         {
//             this.RuleFor(x => x.PartyId).GreaterThan(0);
//             this.RuleFor(x => x.Token).NotEmpty();
//         }
//     }

//     public class CommandHandler : ICommandHandler<Command, IDomainResult>
//     {
//         private readonly ILogger logger;
//         private readonly PidpDbContext context;

//         public CommandHandler(ILogger<CommandHandler> logger, PidpDbContext context)
//         {
//             this.logger = logger;
//             this.context = context;
//         }

//         public async Task<IDomainResult> HandleAsync(Command command)
//         {
//             var receivedRequest = await this.context.EndorsementRequests
//                 .Where(request => request.Token == command.Token)
//                 .SingleOrDefaultAsync();

//             if (receivedRequest == null)
//             {
//                 return DomainResult.NotFound();
//             }
//             if (receivedRequest.Status != EndorsementRequestStatus.Created)
//             {
//                 // Already received
//                 return DomainResult.Failed();
//             }
//             if (receivedRequest.RequestingPartyId == command.PartyId)
//             {
//                 this.logger.LogSelfEndorsementAttempt(command.PartyId);
//                 return DomainResult.Failed();
//             }

//             receivedRequest.ReceivingPartyId = command.PartyId;
//             receivedRequest.Status = EndorsementRequestStatus.Received;

//             await this.context.SaveChangesAsync();

//             return DomainResult.Success();
//         }
//     }
// }

// public static partial class EndorsementRequestReceiveLoggingExtensions
// {
//     [LoggerMessage(1, LogLevel.Warning, "Possible fraudulent behaviour: Party {partyId} received an Endorsement Request from themselves.")]
//     public static partial void LogSelfEndorsementAttempt(this ILogger logger, int partyId);
// }
