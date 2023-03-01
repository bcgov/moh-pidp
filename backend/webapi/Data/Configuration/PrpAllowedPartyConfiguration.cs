namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class PrpAllowedPartyConfiguration : IEntityTypeConfiguration<PrpAllowedParty>
{
    public virtual void Configure(EntityTypeBuilder<PrpAllowedParty> builder)
    {
        builder.HasIndex(x => x.LicenceNumber)
            .IsUnique();
    }
}
