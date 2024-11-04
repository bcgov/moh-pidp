namespace Pidp.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pidp.Models;

public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public virtual void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.HasIndex(x => x.UserId)
            .HasFilter(@$"""UserId"" != '{Guid.Empty}'") // New BC Provider Credentials have not yet signed into Keycloak, and so have a UserId of Guid.Empty.
            .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint("CHK_Credential_AtLeastOneIdentifier",
            @$"((""UserId"" != '{Guid.Empty}') or (""IdpId"" is not null))"));
    }
}
