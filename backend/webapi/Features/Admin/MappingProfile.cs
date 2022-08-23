namespace Pidp.Features.Admin;

using AutoMapper;

using Pidp.Models;
using Pidp.Models.Lookups;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<Party, PartyIndex.Model>()
            .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.ProviderCollegeCode, opt => opt.MapFrom(src => src.LicenceDeclaration!.CollegeCode))
            .ForMember(dest => dest.SAEformsAccessRequest, opt => opt.MapFrom(src => src.AccessRequests.Any(accessRequest => accessRequest.AccessTypeCode == AccessTypeCode.SAEforms)));
    }
}
