namespace Pidp.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pidp.Models;

public class PharmacyConfiguration : IEntityTypeConfiguration<Pharmacy>
{
    public void Configure(EntityTypeBuilder<Pharmacy> builder)
    {
        builder.HasIndex(x => x.PharmaCareCode).IsUnique();
    }
}
