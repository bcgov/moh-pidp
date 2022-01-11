namespace Pidp.Data;

using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Models;

public class PidpDbContext : DbContext
{
    private readonly IClock clock;

    public PidpDbContext(DbContextOptions<PidpDbContext> options, IClock clock)
        : base(options) => this.clock = clock;

    public DbSet<Facility> Facilities { get; set; } = default!;
    public DbSet<Party> Parties { get; set; } = default!;
    public DbSet<PartyCertification> PartyCertifications { get; set; } = default!;

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
    }

    private void ApplyAudits()
    {
        this.ChangeTracker.DetectChanges();
        var updated = this.ChangeTracker.Entries()
            .Where(x => x.Entity is BaseAuditable
                && (x.State == EntityState.Added || x.State == EntityState.Modified));

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
}
