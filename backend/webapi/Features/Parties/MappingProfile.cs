namespace Pidp.Features.Parties;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Party, CollegeCertification.Command>();
        this.CreateMap<PartyCertification, CollegeCertification.Command>();
        this.CreateMap<Party, Demographics.Command>();
        this.CreateMap<Party, WorkSetting.Command>()
            .ForMember(dest => dest.FacilityName, opt =>
            {
                opt.PreCondition(src => src.Facility != null);
                opt.MapFrom(src => src.Facility.FacilityName);
            })
            .ForMember(dest => dest.PhysicalAddress, opt =>
            {
                opt.PreCondition(src => src.Facility != null);
                opt.MapFrom(src => src.Facility.PhysicalAddress);
            });
        this.CreateMap<Facility, WorkSetting.Command>();
        this.CreateMap<FacilityAddress, WorkSetting.Command.Address>();
    }
}
