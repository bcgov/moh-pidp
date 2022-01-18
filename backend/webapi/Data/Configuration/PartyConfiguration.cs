namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class PartyConfiguration : IEntityTypeConfiguration<Party>
{
    public virtual void Configure(EntityTypeBuilder<Party> builder)
    {
        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasIndex(x => x.Hpdid)
            .IsUnique();
    }
}
