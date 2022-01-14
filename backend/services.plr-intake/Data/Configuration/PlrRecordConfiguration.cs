namespace PlrIntake.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlrIntake.Models;

public class PlrRecordConfiguration : IEntityTypeConfiguration<PlrRecord>
{
    public void Configure(EntityTypeBuilder<PlrRecord> builder)
    {
        builder.HasIndex(x => x.Ipc)
            .IsUnique();
    }
}
