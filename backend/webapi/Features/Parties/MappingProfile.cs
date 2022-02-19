namespace Pidp.Features.Parties;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TODO use CreateProjection instead
        this.CreateMap<Party, Demographics.Command>();
        this.CreateMap<Party, ProfileStatus.CommandHandler.ProfileDto>()
            .IncludeMembers(party => party.PartyCertification)
            .ForMember(dest => dest.CompletedEnrolments, opt => opt.MapFrom(src => src.AccessRequests.Select(x => x.AccessType)))
            // .ForMember(dest => dest.FacilityStreet, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress!.Street))
            .ForMember(dest => dest.PlrRecordStatus, opt => opt.Ignore());
        this.CreateMap<Party, WorkSetting.Command>()
            .ForMember(dest => dest.PhysicalAddress, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress));

        this.CreateMap<FacilityAddress, WorkSetting.Command.Address>();
        this.CreateMap<PartyCertification, CollegeCertification.Command>();
        this.CreateMap<PartyCertification, ProfileStatus.CommandHandler.ProfileDto>();
    }
}
