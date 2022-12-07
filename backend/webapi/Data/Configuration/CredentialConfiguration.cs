namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pidp.Models;

public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public virtual void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.Property(x => x.CredentialType)
            .HasDefaultValue(0);
    }
}
