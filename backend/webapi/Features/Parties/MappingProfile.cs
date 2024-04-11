namespace Pidp.Features.Parties;

using AutoMapper;
using NodaTime;
using static NodaTime.Extensions.DateTimeExtensions;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<Party, Demographics.Command>();
        this.CreateProjection<Party, ProfileStatus.ProfileData>()
            .ForMember(dest => dest.CollegeCode, opt => opt.MapFrom(src => src.LicenceDeclaration!.CollegeCode))
            .ForMember(dest => dest.CompletedEnrolments, opt => opt.MapFrom(src => src.AccessRequests.Select(x => x.AccessTypeCode)))
            .ForMember(dest => dest.DemographicsComplete, opt => opt.MapFrom(src => src.Email != null && src.Phone != null))
            .ForMember(dest => dest.HasBCProviderCredential, opt => opt.MapFrom(src => src.Credentials.Any(x => x.IdentityProvider == IdentityProviders.BCProvider)))
            .ForMember(dest => dest.LicenceDeclarationComplete, opt => opt.MapFrom(src => src.LicenceDeclaration != null));

        this.CreateProjection<PartyLicenceDeclaration, LicenceDeclaration.Command>();

        this.CreateMap<PlrRecord, CollegeCertifications.Model>()
            .ForMember(dest => dest.IsGoodStanding, opt => opt.MapFrom(src => src.IsGoodStanding()))
            .ForMember(dest => dest.StatusStartDate, opt => opt.MapFrom(src => src.StatusStartDate != null ? src.StatusStartDate.Value.ToLocalDateTime().Date : (LocalDate?)null));
    }
}
