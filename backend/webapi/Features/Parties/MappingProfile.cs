namespace Pidp.Features.Parties;

using AutoMapper;
using static NodaTime.Extensions.DateTimeExtensions;

using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateProjection<Party, Demographics.Command>();
        this.CreateProjection<Party, ProfileStatus.ProfileStatusDto>()
            // .IncludeMembers(party => party.LicenceDeclaration)
            .ForMember(dest => dest.CompletedEnrolments, opt => opt.MapFrom(src => src.AccessRequests.Select(x => x.AccessTypeCode)))
            .ForMember(dest => dest.OrganizationDetailEntered, opt => opt.MapFrom(src => src.OrgainizationDetail != null))
            .ForMember(dest => dest.PlrGoodStanding, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
        this.CreateProjection<LicenceDeclaration, ProfileStatus.ProfileStatusDto.LicenceDeclarationStatus>()
            .ForMember(dest => dest.NoLicence, opt => opt.MapFrom(src => src.CollegeCode == null));
        this.CreateProjection<Party, WorkSetting.Command>()
            .ForMember(dest => dest.PhysicalAddress, opt => opt.MapFrom(src => src.Facility!.PhysicalAddress));

        this.CreateProjection<FacilityAddress, WorkSetting.Command.Address>();
        this.CreateProjection<LicenceDeclaration, CollegeCertification.Command>();
        this.CreateProjection<PartyAccessAdministrator, AccessAdministrator.Command>();
        this.CreateProjection<PartyOrgainizationDetail, OrganizationDetails.Command>();

        this.CreateMap<PlrRecord, CollegeCertificationIndex.Model>()
            .AfterMap((record, model) =>
            {
                model.IsGoodStanding = record.IsGoodStanding();
                model.StatusStartDate = record.StatusStartDate.HasValue
                    ? record.StatusStartDate.Value.ToLocalDateTime().Date
                    : null;
            });
    }
}
