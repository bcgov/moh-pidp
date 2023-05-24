namespace Pidp.Features.AccessRequests;

using AutoMapper;

using Pidp.Models;
using Pidp.Models.Lookups;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<Party, MSTeamsClinicMember.CommandHandler.EnrolmentDto>()
            .ForMember(dest => dest.AlreadyEnroled, opt => opt.MapFrom(src => src.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.MSTeamsClinicMember)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        this.CreateProjection<Party, MSTeamsPrivacyOfficer.CommandHandler.EnrolmentDto>()
            .ForMember(dest => dest.AlreadyEnroled, opt => opt.MapFrom(src => src.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.MSTeamsPrivacyOfficer)));

        this.CreateMap<MSTeamsClinic, MSTeamsClinicMember.CommandHandler.ClinicDto>()
            .ForMember(dest => dest.PrivacyOfficerName, opt => opt.MapFrom(src => src.PrivacyOfficer!.FullName));
        this.CreateMap<MSTeamsClinicAddress, MSTeamsClinicMember.CommandHandler.ClinicDto.ClinicAddressDto>();
        this.CreateMap<MSTeamsPrivacyOfficer.Command.Address, MSTeamsClinicAddress>();
        this.CreateMap<MSTeamsPrivacyOfficer.Command.Address, MSTeamsClinicAddress>();
    }
}
