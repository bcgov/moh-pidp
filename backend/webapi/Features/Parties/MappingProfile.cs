namespace Pidp.Features.Parties;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Party, Demographics.Command>();
        this.CreateMap<Party, ProfileStatus.Model>()
            .IncludeMembers(party => party.PartyCertification)
            .ForMember(dest => dest.FacilityStreet, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress!.Street));
        this.CreateMap<Party, WorkSetting.Command>()
            .ForMember(dest => dest.PhysicalAddress, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress));

        this.CreateMap<FacilityAddress, WorkSetting.Command.Address>();
        this.CreateMap<PartyCertification, CollegeCertification.Command>();
        this.CreateMap<PartyCertification, ProfileStatus.Model>();
    }
}
