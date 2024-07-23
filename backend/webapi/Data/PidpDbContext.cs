namespace Pidp.Data;

using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Models;

public class PidpDbContext : DbContext
{
    private readonly IClock clock;
    private readonly IMediator mediator;

    public PidpDbContext(
        DbContextOptions<PidpDbContext> options,
        IClock clock,
        IMediator mediator)
        : base(options)
    {
        this.clock = clock;
        this.mediator = mediator;
    }

    public DbSet<AccessRequest> AccessRequests { get; set; } = default!;
    public DbSet<BusinessEvent> BusinessEvents { get; set; } = default!;
    public DbSet<ClientLog> ClientLogs { get; set; } = default!;
    public DbSet<Credential> Credentials { get; set; } = default!;
    public DbSet<CredentialLinkErrorLog> CredentialLinkErrorLogs { get; set; } = default!;
    public DbSet<CredentialLinkTicket> CredentialLinkTickets { get; set; } = default!;
    public DbSet<EmailLog> EmailLogs { get; set; } = default!;
    public DbSet<EndorsementRelationship> EndorsementRelationships { get; set; } = default!;
    public DbSet<EndorsementRequest> EndorsementRequests { get; set; } = default!;
    public DbSet<Endorsement> Endorsements { get; set; } = default!;
    public DbSet<HcimAccountTransfer> HcimAccountTransfers { get; set; } = default!;
    public DbSet<MSTeamsClinic> MSTeamsClinics { get; set; } = default!;
    public DbSet<MSTeamsClinicMemberEnrolment> MSTeamsClinicMemberEnrolments { get; set; } = default!;
    public DbSet<PartyLicenceDeclaration> PartyLicenceDeclarations { get; set; } = default!;
    public DbSet<Party> Parties { get; set; } = default!;
    public DbSet<FhirMessage> FhirMessages { get; set; } = default!;
    public DbSet<PrpAuthorizedLicence> PrpAuthorizedLicences { get; set; } = default!;

    /// <summary>
    /// Do not use. Use SaveChangesAsync Instead.
    /// </summary>
    public override int SaveChanges() => this.SaveChangesAsync().GetAwaiter().GetResult();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await this.DispatchDomainEventsAsync();
        this.ApplyAudits();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<JsonDocument>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PidpDbContext).Assembly);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var eventEntities = this.ChangeTracker.Entries<BaseEntity>()
            .Select(x => x.Entity)
            .Where(entity => entity.DomainEvents.Any());

        foreach (var entity in eventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();

            foreach (var domainEvent in events)
            {
                await this.mediator.Publish(domainEvent);
            }
        }
    }

    private void ApplyAudits()
    {
        var updated = this.ChangeTracker.Entries<BaseAuditable>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);

        var currentInstant = this.clock.GetCurrentInstant();

        foreach (var entry in updated)
        {
            entry.CurrentValues[nameof(BaseAuditable.Modified)] = currentInstant;

            if (entry.State == EntityState.Added)
            {
                entry.CurrentValues[nameof(BaseAuditable.Created)] = currentInstant;
            }
            else
            {
                entry.Property(nameof(BaseAuditable.Created)).IsModified = false;
            }
        }
    }

    // Uncomment for SQL logging
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.LogTo(Console.WriteLine);
}
