namespace Pidp.Features.Parties;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Party, Demographics.Command>();
        this.CreateMap<Party, EnrolmentStatus.Model>();
        this.CreateMap<Party, ProfileStatus.Model>()
            .ForMember(dest => dest.CollegeCode, opt => opt.MapFrom(src => src.PartyCertification!.CollegeCode))
            .ForMember(dest => dest.LicenceNumber, opt => opt.MapFrom(src => src.PartyCertification!.LicenceNumber))
            .ForMember(dest => dest.FacilityStreet, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress!.Street));
        this.CreateMap<Party, WorkSetting.Command>()
            .ForMember(dest => dest.PhysicalAddress, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress));
        this.CreateMap<FacilityAddress, WorkSetting.Command.Address>();

        this.CreateMap<PartyCertification, CollegeCertification.Command>();
    }
}
