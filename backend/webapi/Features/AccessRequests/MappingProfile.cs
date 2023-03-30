namespace Pidp.Features.AccessRequests;

using AutoMapper;

using Pidp.Models;
using Pidp.Models.Lookups;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<Party, MSTeamsPrivacyOfficer.CommandHandler.EnrolmentDto>()
            .ForMember(dest => dest.AlreadyEnroled, opt => opt.MapFrom(src => src.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.MSTeamsPrivacyOfficer)));

        this.CreateMap<MSTeamsPrivacyOfficer.Command.Address, MSTeamsClinicAddress>();
    }
}
