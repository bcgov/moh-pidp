namespace Pidp.Features.Endorsements;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<MSTeamsClinic, MSTeamsPrivacyOfficers.Model>()
            .ForMember(dest => dest.ClinicId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ClinicAddress, opt => opt.MapFrom(src => src.Address));
        this.CreateProjection<MSTeamsClinicAddress, MSTeamsPrivacyOfficers.Model.Address>();
    }
}
