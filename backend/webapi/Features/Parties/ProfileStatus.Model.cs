namespace Pidp.Features.Parties;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public partial class ProfileStatus
{
    public partial class Model
    {
        public enum Alert
        {
            TransientError = 1,
            PlrBadStanding,
            PendingEndorsementRequest
        }

        public enum StatusCode
        {
            Incomplete = 1,
            Complete,
            Locked,
            Error,
            Hidden
        }

        public abstract class ProfileSection
        {
            internal abstract string SectionName { get; }
            public HashSet<Alert> Alerts { get; set; } = new();
            public StatusCode StatusCode { get; set; }

            public bool IsComplete => this.StatusCode == StatusCode.Complete;

            protected ProfileSection() { }

            protected virtual StatusCode Compute(ProfileData profile) => StatusCode.Complete;

            public static TSection Create<TSection>(ProfileData profile) where TSection : ProfileSection, new()
            {
                var section = new TSection();
                section.StatusCode = section.Compute(profile);
                return section;
            }
        }

        public class DashboardInfoSection : ProfileSection
        {
            internal override string SectionName => "dashboardInfo";
            public string DisplayFullName { get; set; } = string.Empty;
            public CollegeCode? CollegeCode { get; set; }

            protected override StatusCode Compute(ProfileData profile)
            {
                this.DisplayFullName = profile.DisplayFullName;
                this.CollegeCode = profile.CollegeCode;

                return StatusCode.Complete; // Unused
            }
        }

        public class BCProviderSection : ProfileSection
        {
            internal override string SectionName => "bcProvider";

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Hidden,
                    { HasBCProviderCredential: true } => StatusCode.Complete,
                    _ => StatusCode.Incomplete
                };
            }
        }

        public class CollegeCertificationSection : ProfileSection
        {
            internal override string SectionName => "collegeCertification";
            public bool HasCpn { get; set; }
            public bool LicenceDeclared { get; set; }

            protected override StatusCode Compute(ProfileData profile)
            {
                this.HasCpn = !string.IsNullOrWhiteSpace(profile.Cpn);
                this.LicenceDeclared = !profile.HasNoLicence;

                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (profile.HasNoLicence || profile.PartyPlrStanding.HasGoodStanding)
                {
                    return StatusCode.Complete;
                }

                if (profile.PartyPlrStanding.Error)
                {
                    this.Alerts.Add(Alert.TransientError);
                    return StatusCode.Error;
                }

                this.Alerts.Add(Alert.PlrBadStanding);
                return StatusCode.Error;
            }
        }

        public class DemographicsSection : ProfileSection
        {
            internal override string SectionName => "demographics";
        }

        public class EndorsementsSection : ProfileSection
        {
            internal override string SectionName => "endorsements";

            protected override StatusCode Compute(ProfileData profile)
            {
                if (profile.HasPendingEndorsementRequest)
                {
                    this.Alerts.Add(Alert.PendingEndorsementRequest);
                }

                return profile.UserIsHighAssuranceIdentity
                    ? StatusCode.Incomplete
                    : StatusCode.Hidden;
            }
        }

        public class UserAccessAgreementSection : ProfileSection
        {
            internal override string SectionName => "userAccessAgreement";
        }

        public class DriverFitnessSection : ProfileSection
        {
            internal override string SectionName => "driverFitness";

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    {UserIsHighAssuranceIdentity: false} => StatusCode.Hidden,
                    CompletedEnrolments.Contains
                }

                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.DriverFitness))
                {
                    return StatusCode.Complete;
                }

                if (profile.PartyPlrStanding
                    .With(DriverFitness.AllowedIdentifierTypes)
                    .HasGoodStanding
                    || (profile.HasNoLicence && profile.EndorsementPlrStanding.HasGoodStanding))
                {
                    return StatusCode.Incomplete;
                }

                return StatusCode.Locked;
            }
        }

        public class HcimAccountTransferSection : ProfileSection
        {
            internal override string SectionName => "hcimAccountTransfer";

            protected override StatusCode Compute(ProfileData profile)
            {
                // TODO revert [
                // if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimAccountTransfer)
                //    || profile.CompletedEnrolments.Contains(AccessTypeCode.HcimEnrolment))
                // {
                //     this.StatusCode = StatusCode.Hidden;
                //     return;
                // }
                if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimAccountTransfer))
                {
                    return StatusCode.Complete;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimEnrolment))
                {
                    return StatusCode.Hidden;
                }
                // ]

                return StatusCode.Incomplete;
            }
        }

        public class HcimEnrolmentSection : ProfileSection
        {
            internal override string SectionName => "hcimEnrolment";

            protected override StatusCode Compute(ProfileData profile)
            {
                // TODO revert [
                // if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimAccountTransfer)
                //     || profile.CompletedEnrolments.Contains(AccessTypeCode.HcimEnrolment))
                // {
                //     this.StatusCode = StatusCode.Complete;
                //     return;
                // }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimAccountTransfer))
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimEnrolment))
                {
                    return StatusCode.Complete;
                }
                // ]

                return StatusCode.Locked; // string.IsNullOrWhiteSpace(profile.AccessAdministratorEmail)
            }
        }

        public class ImmsBCEformsSection : ProfileSection
        {
            internal override string SectionName => "immsBCEforms";

            protected override StatusCode Compute(ProfileData profile)
            {
                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.ImmsBCEforms))
                {
                    return StatusCode.Complete;
                }

                return profile.PartyPlrStanding.HasGoodStanding
                    ? StatusCode.Incomplete
                    : StatusCode.Locked;
            }
        }

        public class MSTeamsClinicMemberSection : ProfileSection
        {
            internal override string SectionName => "msTeamsClinicMember";

            protected override StatusCode Compute(ProfileData profile)
            {
                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.MSTeamsClinicMember))
                {
                    return StatusCode.Complete;
                }

                return profile.HasMSTeamsClinicEndorsement
                    ? StatusCode.Incomplete
                    : StatusCode.Locked;
            }
        }

        public class MSTeamsPrivacyOfficerSection : ProfileSection
        {
            internal override string SectionName => "msTeamsPrivacyOfficer";

            protected override StatusCode Compute(ProfileData profile)
            {
                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.MSTeamsPrivacyOfficer))
                {
                    return StatusCode.Complete;
                }

                return profile.PartyPlrStanding
                    .With(MSTeamsPrivacyOfficer.AllowedIdentifierTypes)
                    .HasGoodStanding
                        ? StatusCode.Incomplete
                        : StatusCode.Locked;
            }
        }

        public class PrescriptionRefillEformsSection : ProfileSection
        {
            internal override string SectionName => "prescriptionRefillEforms";

            protected override StatusCode Compute(ProfileData profile)
            {
                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.PrescriptionRefillEforms))
                {
                    return StatusCode.Complete;
                }

                if (profile.PartyPlrStanding
                    .With(PrescriptionRefillEforms.AllowedIdentifierTypes)
                    .HasGoodStanding)
                {
                    return StatusCode.Incomplete;
                }

                return StatusCode.Locked;
            }
        }

        public class PrimaryCareRosteringSection : ProfileSection
        {
            internal override string SectionName => "primaryCareRostering";

            protected override StatusCode Compute(ProfileData profile)
            {
                if ((profile.EndorsementPlrStanding.HasGoodStanding
                    || profile.PartyPlrStanding
                        .With(ProviderRoleType.MedicalDoctor, ProviderRoleType.RegisteredNursePractitioner)
                        .HasGoodStanding)
                    && profile.HasBCProviderCredential
                    && profile.CompletedEnrolments.Contains(AccessTypeCode.UserAccessAgreement))
                {
                    return StatusCode.Incomplete;
                }

                return StatusCode.Locked;
            }
        }

        public class ProviderReportingPortalSection : ProfileSection
        {
            internal override string SectionName => "providerReportingPortal";

            protected override StatusCode Compute(ProfileData profile)
            {
                if (!profile.UserIsBCProvider || !profile.HasPrpAuthorizedLicence)
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.ProviderReportingPortal))
                {
                    return StatusCode.Complete;
                }

                if (profile.PartyPlrStanding
                    .With(ProviderReportingPortal.AllowedIdentifierTypes)
                    .HasGoodStanding)
                {
                    return StatusCode.Incomplete;
                }

                return StatusCode.Locked;
            }
        }

        public class SAEformsSection : ProfileSection
        {
            internal override string SectionName => "saEforms";
            public bool IncorrectLicenceType { get; set; }

            protected override StatusCode Compute(ProfileData profile)
            {
                this.IncorrectLicenceType = profile.PartyPlrStanding.HasGoodStanding
                    && !profile.PartyPlrStanding
                        .Excluding(SAEforms.ExcludedIdentifierTypes)
                        .HasGoodStanding;

                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.SAEforms))
                {
                    return StatusCode.Complete;
                }

                if (profile.PartyPlrStanding
                    .Excluding(SAEforms.ExcludedIdentifierTypes)
                    .HasGoodStanding)
                {
                    return StatusCode.Incomplete;
                }

                return StatusCode.Locked;
            }
        }
    }
}
