namespace PlrIntake.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlrIntake.Models;

public class PlrRecordConfiguration : IEntityTypeConfiguration<PlrRecord>
{
    public void Configure(EntityTypeBuilder<PlrRecord> builder)
    {
        builder.HasIndex(x => x.Cpn);
        builder.HasIndex(x => x.Ipc)
            .IsUnique();

        builder.OwnsMany(x => x.Credentials, credential => credential.ToTable(nameof(Credential)));
        builder.OwnsMany(x => x.Expertise, expertise => expertise.ToTable(nameof(Expertise)));
    }
}
