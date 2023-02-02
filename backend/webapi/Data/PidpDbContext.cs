namespace Pidp.Data;

using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Models;

public class PidpDbContext : DbContext
{
    private readonly IClock clock;

    public PidpDbContext(DbContextOptions<PidpDbContext> options, IClock clock) : base(options) => this.clock = clock;

    public DbSet<AccessRequest> AccessRequests { get; set; } = default!;
    public DbSet<BusinessEvent> BusinessEvents { get; set; } = default!;
    public DbSet<ClientLog> ClientLogs { get; set; } = default!;
    public DbSet<Credential> Credentials { get; set; } = default!;
    public DbSet<EmailLog> EmailLogs { get; set; } = default!;
    public DbSet<EndorsementRelationship> EndorsementRelationships { get; set; } = default!;
    public DbSet<EndorsementRequest> EndorsementRequests { get; set; } = default!;
    public DbSet<Endorsement> Endorsements { get; set; } = default!;
    public DbSet<Facility> Facilities { get; set; } = default!;
    public DbSet<HcimAccountTransfer> HcimAccountTransfers { get; set; } = default!;
    public DbSet<HcimEnrolment> HcimEnrolments { get; set; } = default!;
    public DbSet<MSTeamsEnrolment> MSTeamsEnrolments { get; set; } = default!;
    public DbSet<PartyLicenceDeclaration> PartyLicenceDeclarations { get; set; } = default!;
    public DbSet<Party> Parties { get; set; } = default!;
    public DbSet<PartyAccessAdministrator> PartyAccessAdministrators { get; set; } = default!;
    public DbSet<PartyOrgainizationDetail> PartyOrgainizationDetails { get; set; } = default!;

    public override int SaveChanges()
    {
        this.ApplyAudits();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.ApplyAudits();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PidpDbContext).Assembly);
        modelBuilder.Entity<PartyNotInPlr>(); // We must make the context aware of types. Since business events are not referenced on any models and we don't want to make a DB Set for each type of event; here we are.
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
