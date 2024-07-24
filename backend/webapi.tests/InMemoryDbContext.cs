namespace Pidp.Data;

using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

public class InMemoryDbContext : PidpDbContext
{

    public InMemoryDbContext(DbContextOptions<PidpDbContext> options, IClock clock, IMediator mediator) : base(options, clock, mediator)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<JsonDocument>();
    }

}
