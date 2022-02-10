namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public virtual void Configure(EntityTypeBuilder<Address> builder) => builder.HasDiscriminator();
}
