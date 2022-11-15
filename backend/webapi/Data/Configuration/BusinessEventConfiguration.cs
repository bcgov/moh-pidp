namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class BusinessEventConfiguration : IEntityTypeConfiguration<BusinessEvent>
{
    public virtual void Configure(EntityTypeBuilder<BusinessEvent> builder) => builder.HasDiscriminator();
}
