namespace SpikeWebApi; 

using Microsoft.EntityFrameworkCore;

public class SpikeDbContext : DbContext
{
    public DbSet<SpikeParty> SpikeParty { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5442;Database=postgres;Username=postgres;Password=postgres");
        optionsBuilder.LogTo(Console.WriteLine);
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);

    //     modelBuilder
    //     .Entity<SpikeParty>()
    //     .OwnsOne(party => party.PlrRecord, 
    //             builder => { 
    //                 builder.ToJson(); 
    //             });
    // }
}