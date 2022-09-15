namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class EndorsementRequestConfiguration : IEntityTypeConfiguration<EndorsementRequest>
{
    public virtual void Configure(EntityTypeBuilder<EndorsementRequest> builder)
    {
        builder.HasOne(x => x.ReceivingParty)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
