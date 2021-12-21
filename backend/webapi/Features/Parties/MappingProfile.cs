namespace Pidp.Features.Parties;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Party, Demographics.Command>();
        this.CreateMap<PartyAddress, Demographics.Command.Address>();
        this.CreateMap<PartyCertification, CollegeCertification.Command>();
        this.CreateMap<Party, WorkSetting.Command>()
            .IncludeMembers(party => party.Facility);
        this.CreateMap<Facility, WorkSetting.Command>();
        this.CreateMap<FacilityAddress, WorkSetting.Command.Address>();
    }
}
