namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

using Pidp.Models;

public class BusinessEventConfiguration : IEntityTypeConfiguration<BusinessEvent>
{
    public virtual void Configure(EntityTypeBuilder<BusinessEvent> builder)
    {
        var businessEventTypes = Assembly.GetAssembly(typeof(BusinessEvent))!
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(BusinessEvent))
                && !type.IsAbstract);

        var discriminatorBuilder = builder.HasDiscriminator();

        foreach (var type in businessEventTypes)
        {
            discriminatorBuilder.HasValue(type, type.Name);
        }
    }
}
