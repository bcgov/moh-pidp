namespace Pidp.Features.Parties.ProfileStatusInternal;

using static Pidp.Features.Parties.ProfileStatus;

public interface IProfileSectionResolver
{
    void ComputeSection(Model profileStatus, CommandHandler.ProfileDto profile);
}
