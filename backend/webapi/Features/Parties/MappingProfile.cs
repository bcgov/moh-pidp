namespace Pidp.Features.Parties;

using AutoMapper;
using NodaTime;
using static NodaTime.Extensions.DateTimeExtensions;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Infrastructure.Auth;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<Party, Demographics.Command>();
        this.CreateProjection<Party, ProfileStatus.ProfileData>()
            .ForMember(dest => dest.CompletedEnrolments, opt => opt.MapFrom(src => src.AccessRequests.Select(x => x.AccessTypeCode)))
            .ForMember(dest => dest.HasBCProviderCredential, opt => opt.MapFrom(src => src.Credentials.Any(x => x.IdentityProvider == IdentityProviders.BCProvider)))
            .ForMember(dest => dest.OrganizationDetailEntered, opt => opt.MapFrom(src => src.OrgainizationDetail != null))
            .ForMember(dest => dest.EndorsementPlrStanding, opt => opt.Ignore())
            .ForMember(dest => dest.PartyPlrStanding, opt => opt.Ignore());
        this.CreateProjection<Party, WorkSetting.Command>()
            .ForMember(dest => dest.PhysicalAddress, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress));

        this.CreateProjection<FacilityAddress, WorkSetting.Command.Address>();
        this.CreateProjection<PartyAccessAdministrator, AccessAdministrator.Command>();
        this.CreateProjection<PartyLicenceDeclaration, LicenceDeclaration.Command>();
        this.CreateProjection<PartyLicenceDeclaration, ProfileStatus.ProfileData.LicenceDeclarationDto>();
        this.CreateProjection<PartyOrgainizationDetail, OrganizationDetails.Command>();

        this.CreateMap<PlrRecord, CollegeCertifications.Model>()
            .ForMember(dest => dest.IsGoodStanding, opt => opt.MapFrom(src => src.IsGoodStanding()))
            .ForMember(dest => dest.StatusStartDate, opt => opt.MapFrom(src => src.StatusStartDate != null ? src.StatusStartDate.Value.ToLocalDateTime().Date : (LocalDate?)null));
    }
}
