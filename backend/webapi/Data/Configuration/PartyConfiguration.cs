namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class PartyConfiguration : IEntityTypeConfiguration<Party>
{
    public virtual void Configure(EntityTypeBuilder<Party> builder)
    {
        builder.HasIndex(x => x.OpId)
            .IsUnique();
    }
}
