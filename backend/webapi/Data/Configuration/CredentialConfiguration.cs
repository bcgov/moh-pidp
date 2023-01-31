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

        builder.HasCheckConstraint("CHK_Credential_AtLeastOneIdentifier",
            @"((""UserId"" is not null) or (""IdpId"" is not null))");
    }
}
