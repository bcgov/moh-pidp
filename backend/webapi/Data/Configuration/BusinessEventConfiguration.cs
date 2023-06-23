namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Extensions;
using Pidp.Models;

public class BusinessEventConfiguration : IEntityTypeConfiguration<BusinessEvent>
{
    public virtual void Configure(EntityTypeBuilder<BusinessEvent> builder)
    {
        var discriminatorBuilder = builder.HasDiscriminator();

        foreach (var type in typeof(BusinessEvent).GetDerivedTypes())
        {
            discriminatorBuilder.HasValue(type, type.Name);
        }
    }
}
