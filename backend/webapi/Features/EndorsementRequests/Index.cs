namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public int Id { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; }
        public string? PartyName { get; set; }
        public CollegeCode? CollegeCode { get; set; }
        public EndorsementRequestStatus Status { get; set; }
        public Instant StatusDate { get; set; }
        public bool Actionable { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            var requested = await this.context.EndorsementRequests
                .Where(request => request.RequestingPartyId == query.PartyId
                    && request.Status != EndorsementRequestStatus.Expired
                    && request.Status != EndorsementRequestStatus.Completed) // Endorsement Requests that successfully complete are now full Endorsements
                .Select(request => new Model
                {
                    Id = request.Id,
                    RecipientEmail = request.RecipientEmail,
                    AdditionalInformation = request.AdditionalInformation,
                    PartyName = request.ReceivingParty == null
                        ? null
                        : request.ReceivingParty.FullName,
                    CollegeCode = request.ReceivingParty!.LicenceDeclaration!.CollegeCode,
                    Status = request.Status,
                    StatusDate = request.StatusDate,
                    Actionable = request.Status == EndorsementRequestStatus.Approved
                })
                .ToArrayAsync();

            var recieved = await this.context.EndorsementRequests
                .Where(request => request.ReceivingPartyId == query.PartyId
                    && request.Status != EndorsementRequestStatus.Expired
                    && request.Status != EndorsementRequestStatus.Completed)
                .Select(request => new Model
                {
                    Id = request.Id,
                    RecipientEmail = request.RecipientEmail,
                    AdditionalInformation = request.AdditionalInformation,
                    PartyName = request.RequestingParty!.FullName,
                    CollegeCode = request.RequestingParty!.LicenceDeclaration!.CollegeCode,
                    Status = request.Status,
                    StatusDate = request.StatusDate,
                    Actionable = request.Status == EndorsementRequestStatus.Received
                })
                .ToArrayAsync();

            return requested
                .Concat(recieved)
                .OrderByDescending(model => model.StatusDate)
                .ToList();
        }
    }
}
