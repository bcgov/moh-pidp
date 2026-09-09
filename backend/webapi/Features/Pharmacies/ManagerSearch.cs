namespace Pidp.Features.Pharmacies;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mediator;

using Pidp.Data;
using Pidp.Models.Lookups;

public class ManagerSearch
{
    public class Query : IRequest<Model?>
    {
        public string LicenceNumber { get; set; } = string.Empty;
    }

    public class Model
    {
        public int PartyId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            this.RuleFor(x => x.LicenceNumber).NotEmpty();
        }
    }

    public class QueryHandler(PidpDbContext context) : IRequestHandler<Query, Model?>
    {
        private readonly PidpDbContext context = context;

        public async ValueTask<Model?> Handle(Query request, CancellationToken cancellationToken)
        {
            var unpadded = request.LicenceNumber.TrimStart('0');
            var padded = request.LicenceNumber.PadLeft(5, '0');

            return await this.context.Parties
                .Where(party => party.LicenceDeclaration != null
                             && (party.LicenceDeclaration.LicenceNumber == request.LicenceNumber || party.LicenceDeclaration.LicenceNumber == unpadded || party.LicenceDeclaration.LicenceNumber == padded)
                             && party.LicenceDeclaration.CollegeCode == CollegeCode.Pharmacists)
                .Select(party => new Model
                {
                    PartyId = party.Id,
                    FullName = party.FirstName + " " + party.LastName
                })
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
