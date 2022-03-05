namespace Pidp.Features.Admin;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<Party, Index.Model>()
            .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.ProviderCollegeCode, opt => opt.MapFrom(src => src.PartyCertification!.CollegeCode))
            .ForMember(dest => dest.SAEformsAccessRequest, opt => opt.MapFrom(src => src.AccessRequests.Any(accessRequest => accessRequest.AccessType == AccessType.SAEforms)));
    }
}
