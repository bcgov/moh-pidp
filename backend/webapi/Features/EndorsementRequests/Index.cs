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
            // TODO: do this better?
            return await this.context.EndorsementRequests
                .Where(request => request.RequestingPartyId == query.PartyId
                    || request.ReceivingPartyId == query.PartyId)
                .Where(request => request.Status != EndorsementRequestStatus.Completed) // Endorsement Requests that successfully complete are now full Endorsements
                .Select(request => new
                {
                    PartyIsRequester = request.RequestingPartyId == query.PartyId,
                    OtherParty = request.RequestingPartyId == query.PartyId
                        ? request.ReceivingParty
                        : request.RequestingParty,
                    Request = request
                })
                .Select(dto => new Model
                {
                    Id = dto.Request.Id,
                    RecipientEmail = dto.Request.RecipientEmail,
                    AdditionalInformation = dto.Request.AdditionalInformation,
                    PartyName = dto.OtherParty == null ? null : $"{dto.OtherParty.FirstName} {dto.OtherParty.LastName}",
                    CollegeCode = dto.OtherParty!.LicenceDeclaration!.CollegeCode,
                    Status = dto.Request.Status,
                    StatusDate = dto.Request.StatusDate,
                    Actionable = (dto.PartyIsRequester && dto.Request.Status == EndorsementRequestStatus.Approved)
                        || (!dto.PartyIsRequester && dto.Request.Status == EndorsementRequestStatus.Received)
                })
                .OrderByDescending(model => model.StatusDate)
                .ToListAsync();
        }
    }
}
