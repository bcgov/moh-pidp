namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class PrpAuthorizedLicenceConfiguration : IEntityTypeConfiguration<PrpAuthorizedLicence>
{
    public virtual void Configure(EntityTypeBuilder<PrpAuthorizedLicence> builder)
    {
        builder.HasIndex(x => x.LicenceNumber)
            .IsUnique();
    }
}
