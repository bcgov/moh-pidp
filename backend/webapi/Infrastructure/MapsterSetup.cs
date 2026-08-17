namespace Pidp.Infrastructure;

using Mapster;
using NodaTime;
using static NodaTime.Extensions.DateTimeExtensions;
using Pidp.Models;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Features.Parties;
using Pidp.Features.Endorsements;
using Pidp.Features.Admin;
using Pidp.Features.AccessRequests;

public static class MapsterSetup
{
    public static void Configure()
    {
        // Features.Parties
        TypeAdapterConfig<Party, ProfileStatus.ProfileData>.NewConfig()
            .Map(dest => dest.CollegeCode, src => src.LicenceDeclaration != null ? src.LicenceDeclaration.CollegeCode : null)
            .Map(dest => dest.CompletedEnrolments, src => src.AccessRequests.Select(x => x.AccessTypeCode))
            .Map(dest => dest.DemographicsComplete, src => src.Email != null && src.Phone != null)
            .Map(dest => dest.HasBCProviderCredential, src => src.Credentials.Any(x => x.IdentityProvider == IdentityProviders.BCProvider))
            .Map(dest => dest.HasBCServicesCardCredential, src => src.Credentials.Any(x => x.IdentityProvider == IdentityProviders.BCServicesCard))
            .Map(dest => dest.DisplayFullName, src => (src.PreferredFirstName ?? src.FirstName) + " " + (src.PreferredLastName ?? src.LastName))
            .Map(dest => dest.LicenceDeclarationComplete, src => src.LicenceDeclaration != null);

        TypeAdapterConfig<PlrRecord, CollegeCertifications.Model>.NewConfig()
            .Map(dest => dest.IsGoodStanding, src => src.IsGoodStanding())
            .Map(dest => dest.StatusStartDate, src => src.StatusStartDate != null ? src.StatusStartDate.Value.ToLocalDateTime().Date : (LocalDate?)null);

        // Features.Endorsements
        TypeAdapterConfig<MSTeamsClinic, MSTeamsPrivacyOfficers.Model>.NewConfig()
            .Map(dest => dest.FullName, src => src.PrivacyOfficer!.FirstName + " " + src.PrivacyOfficer.LastName)
            .Map(dest => dest.ClinicId, src => src.Id)
            .Map(dest => dest.ClinicName, src => src.Name)
            .Map(dest => dest.ClinicAddress, src => src.Address);

        // Features.Admin
        TypeAdapterConfig<Party, PartyIndex.Model>.NewConfig()
            .Map(dest => dest.ProviderName, src => src.FullName)
            .Map(dest => dest.ProviderCollegeCode, src => src.LicenceDeclaration != null ? src.LicenceDeclaration.CollegeCode : null)
            .Map(dest => dest.SAEformsAccessRequest, src => src.AccessRequests.Any(accessRequest => accessRequest.AccessTypeCode == AccessTypeCode.SAEforms));

        // Features.AccessRequests
        TypeAdapterConfig<Party, MSTeamsClinicMember.CommandHandler.EnrolmentDto>.NewConfig()
            .Map(dest => dest.AlreadyEnroled, src => src.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.MSTeamsClinicMember))
            .Map(dest => dest.Name, src => src.FirstName + " " + src.LastName);
        
        TypeAdapterConfig<Party, MSTeamsPrivacyOfficer.CommandHandler.EnrolmentDto>.NewConfig()
            .Map(dest => dest.AlreadyEnroled, src => src.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.MSTeamsPrivacyOfficer));

        TypeAdapterConfig<MSTeamsClinic, MSTeamsClinicMember.CommandHandler.ClinicDto>.NewConfig()
            .Map(dest => dest.PrivacyOfficerName, src => src.PrivacyOfficer!.FirstName + " " + src.PrivacyOfficer.LastName);
            
        // Mapster handles 1-to-1 matching automatically, so we don't need empty configs for 
        // MSTeamsClinicAddress, LicenceDeclaration, etc., unless there are custom mappings.
        // TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
    }
}
