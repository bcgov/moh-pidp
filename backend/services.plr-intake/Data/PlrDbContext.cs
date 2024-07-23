namespace PlrIntake.Data;

using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using PlrIntake.Models;

public class PlrDbContext : DbContext
{
    public PlrDbContext(DbContextOptions<PlrDbContext> options) : base(options) { }

    public DbSet<PlrRecord> PlrRecords { get; set; } = default!;
    public DbSet<IdentifierType> IdentifierTypes { get; set; } = default!;
    public DbSet<StatusChageLog> StatusChageLogs { get; set; } = default!;

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
        modelBuilder.Ignore<JsonDocument>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlrDbContext).Assembly);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName($"Plr_{entity.GetTableName()}");
        }
    }

    private void ApplyAudits()
    {
        var updated = this.ChangeTracker.Entries<BaseAuditable>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);

        var currentInstant = DateTime.Now;

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
