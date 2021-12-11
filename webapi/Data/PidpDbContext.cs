namespace Pidp.Data;

using Microsoft.EntityFrameworkCore;

using Pidp.Models;

public class PidpDbContext : DbContext
{
    public PidpDbContext(DbContextOptions<PidpDbContext> options) : base(options) { }

    public DbSet<Party> Parties { get; set; } = default!;

    public override int SaveChanges()
    {
        //this.ApplyAudits();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //this.ApplyAudits();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PidpDbContext).Assembly);
    }
}
