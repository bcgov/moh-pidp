namespace Pidp.Features.Admin;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile() => this.CreateMap<Party, Index.Model>()
        .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
        .ForMember(dest => dest.ProviderCollegeCode, opt => opt.MapFrom(src => src.PartyCertification!.CollegeCode));
}
