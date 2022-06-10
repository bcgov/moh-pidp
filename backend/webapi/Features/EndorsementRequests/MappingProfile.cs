namespace Pidp.Features.EndorsementRequests;

using AutoMapper;

using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<EndorsementRequest, RecievedIndex.Model>()
            .ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => $"{src.RequestingParty!.FirstName} {src.RequestingParty.LastName}"));
    }
}
