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
            return await this.context.EndorsementRequests
                .Where(request => request.RequestingPartyId == query.PartyId
                    || request.ReceivingPartyId == query.PartyId)
                .Where(request => request.Status != EndorsementRequestStatus.Completed) // Endorsement Requests that successfully complete are now full Endorsements
                .Select(request => new Model
                {
                    Id = request.Id,
                    RecipientEmail = request.RecipientEmail,
                    AdditionalInformation = request.AdditionalInformation,
                    PartyName = request.RequestingPartyId == query.PartyId
                        ? request.ReceivingParty!.FullName
                        : request.RequestingParty!.FullName,
                    CollegeCode = request.RequestingPartyId == query.PartyId
                        ? request.ReceivingParty!.LicenceDeclaration!.CollegeCode
                        : request.RequestingParty!.LicenceDeclaration!.CollegeCode,
                    Status = request.Status,
                    StatusDate = request.StatusDate,
                    Actionable = request.RequestingPartyId == query.PartyId
                        ? request.Status == EndorsementRequestStatus.Approved
                        : request.Status == EndorsementRequestStatus.Received
                })
                .OrderByDescending(model => model.StatusDate)
                .ToListAsync();
        }
    }
}
