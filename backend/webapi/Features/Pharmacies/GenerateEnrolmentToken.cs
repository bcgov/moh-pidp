namespace Pidp.Features.Pharmacies;

using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class GenerateEnrolmentToken
{
    public class Command : IRequest<string>
    {
        public int PharmacyId { get; set; }
        public PharmacyRole RoleToAssign { get; set; }
        public int RequestingPartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PharmacyId).GreaterThan(0);
            this.RuleFor(x => x.RoleToAssign).IsInEnum();
            this.RuleFor(x => x.RequestingPartyId).GreaterThan(0);
        }
    }

    public class CommandHandler(PidpDbContext context, IClock clock) : IRequestHandler<Command, string>
    {
        public async ValueTask<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var partyIsAdmin = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.RequestingPartyId
                               && role.PharmacyId == request.PharmacyId
                               && role.Role == PharmacyRole.Admin,
                          cancellationToken);

            if (!partyIsAdmin)
            {
                throw new InvalidOperationException("User is not an admin of this pharmacy.");
            }

            var token = new PharmacyEnrolment
            {
                PharmacyId = request.PharmacyId,
                Role = request.RoleToAssign,
                Token = Guid.NewGuid(),
                EffectiveStartDate = clock.GetCurrentInstant().ToDateTimeUtc(),
                EffectiveEndDate = clock.GetCurrentInstant().Plus(Duration.FromDays(90)).ToDateTimeUtc()
            };

            context.PharmacyEnrolments.Add(token);
            await context.SaveChangesAsync(cancellationToken);

            return token.Token.ToString();
        }
    }
}
