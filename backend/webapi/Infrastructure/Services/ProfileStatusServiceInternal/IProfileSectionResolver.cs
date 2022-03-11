namespace Pidp.Infrastructure.Services.ProfileStatusServiceInternal;

using Pidp.Models.ProfileStatus;

public interface IProfileSectionResolver
{
    public IProfileSection ResolveSection(ProfileStatusDto profile);
}
