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
            public HashSet<Alert> Alerts { get; set; } = [];
            public StatusCode StatusCode { get; set; }
            public virtual string[] KeyWords { get; } = [];

            public bool IsComplete => this.StatusCode == StatusCode.Complete;

            protected ProfileSection() { }

            protected abstract StatusCode Compute(ProfileData profile);

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
            public override string[] KeyWords => ["doctors", "nursing", "ha", "pharmacist"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { HasBCProviderCredential: true } => StatusCode.Complete,
                    { HasBCServicesCardCredential: true } => StatusCode.Incomplete,
                    _ => StatusCode.Locked
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
                this.LicenceDeclared = profile.LicenceDeclarationComplete && !profile.HasNoLicence;

                if (!profile.UserIsHighAssuranceIdentity)
                {
                    return StatusCode.Hidden;
                }

                if (!profile.LicenceDeclarationComplete)
                {
                    return StatusCode.Incomplete;
                }

                if (profile.HasNoLicence || profile.PartyPlrStanding.HasGoodStanding)
                {
                    return StatusCode.Complete;
                }

                // CPS Postgrads do not want to see the bad standing alert
                // however, they are still not considered in Good Standing for all systems
                if (!profile.PartyPlrStanding.IsCpsPostgrad)
                {
                    this.Alerts.Add(profile.PartyPlrStanding.Error
                        ? Alert.TransientError
                        : Alert.PlrBadStanding);
                }

                return StatusCode.Error;
            }
        }

        public class DemographicsSection : ProfileSection
        {
            internal override string SectionName => "demographics";

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile.DemographicsComplete
                    ? StatusCode.Complete
                    : StatusCode.Incomplete;
            }
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

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile.HasEnrolment(AccessTypeCode.UserAccessAgreement)
                    ? StatusCode.Complete
                    : StatusCode.Incomplete;
            }
        }

        public class AccountLinkingSection : ProfileSection
        {
            internal override string SectionName => "accountLinking";
            public override string[] KeyWords => ["doctors", "ha", "nursing"];

            protected override StatusCode Compute(ProfileData profile) => StatusCode.Incomplete;
        }

        public class DriverFitnessSection : ProfileSection
        {
            internal override string SectionName => "driverFitness";
            public override string[] KeyWords => ["doctors"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.DriverFitness) => StatusCode.Complete,
                    _ when profile.PartyPlrStanding
                        .With(DriverFitness.AllowedIdentifierTypes)
                        .HasGoodStanding
                        || (profile.HasNoLicence && profile.EndorsementPlrStanding.HasGoodStanding) => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }

        public class HcimAccountTransferSection : ProfileSection
        {
            internal override string SectionName => "hcimAccountTransfer";
            public override string[] KeyWords => ["ha"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile.HasEnrolment(AccessTypeCode.HcimAccountTransfer)
                    ? StatusCode.Complete
                    : StatusCode.Incomplete;
            }
        }

        public class ImmsBCEformsSection : ProfileSection
        {
            internal override string SectionName => "immsBCEforms";
            public override string[] KeyWords => ["doctors", "nursing", "pharmacist"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.ImmsBCEforms) => StatusCode.Complete,
                    _ when ImmsBCEforms.IsEligible(profile.PartyPlrStanding) ||
                        profile.EndorsementPlrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding ||
                        profile.EndorsementPlrStanding.With(IdentifierType.Nurse).HasGoodStanding => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }

        public class MSTeamsClinicMemberSection : ProfileSection
        {
            internal override string SectionName => "msTeamsClinicMember";
            public override string[] KeyWords => ["ha"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.MSTeamsClinicMember) => StatusCode.Complete,
                    { HasMSTeamsClinicEndorsement: true } => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }

        public class MSTeamsPrivacyOfficerSection : ProfileSection
        {
            internal override string SectionName => "msTeamsPrivacyOfficer";
            public override string[] KeyWords => ["ha"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.MSTeamsPrivacyOfficer) => StatusCode.Complete,
                    _ when profile.PartyPlrStanding
                        .With(MSTeamsPrivacyOfficer.AllowedIdentifierTypes)
                        .HasGoodStanding => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }

        public class PrescriptionRefillEformsSection : ProfileSection
        {
            internal override string SectionName => "prescriptionRefillEforms";
            public override string[] KeyWords => ["pharmacists", "rx"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.PrescriptionRefillEforms) => StatusCode.Complete,
                    _ when profile.PartyPlrStanding
                        .With(PrescriptionRefillEforms.AllowedIdentifierTypes)
                        .HasGoodStanding => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }

        public class ProvincialAttachmentSystemSection : ProfileSection
        {
            internal override string SectionName => "provincialAttachmentSystem";
            public override string[] KeyWords => ["doctors", "nursing", "panel"];

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    _ when (profile.EndorsementPlrStanding.HasGoodStanding
                        || profile.PartyPlrStanding
                            .With(ProviderRoleType.MedicalDoctor, ProviderRoleType.RegisteredNursePractitioner)
                            .HasGoodStanding)
                        && profile.HasBCProviderCredential => StatusCode.Complete,
                    { HasBCServicesCardCredential: true } => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }

         public class IvfSection : ProfileSection
        {
            internal override string SectionName => "ivf";

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.Ivf) && profile.HasBCProviderCredential => StatusCode.Complete,
                    _ when profile.PartyPlrStanding
                        .With(Ivf.AllowedIdentifierTypes)
                        .HasGoodStanding => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }


        public class ProviderReportingPortalSection : ProfileSection
        {
            internal override string SectionName => "providerReportingPortal";

            protected override StatusCode Compute(ProfileData profile)
            {
                return profile switch
                {
                    { HasBCProviderCredential: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.ProviderReportingPortal) => StatusCode.Complete,
                    _ when profile.PartyPlrStanding
                        .With(ProviderReportingPortal.AllowedIdentifierTypes)
                        .HasGoodStanding => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
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

                return profile switch
                {
                    { UserIsHighAssuranceIdentity: false } => StatusCode.Locked,
                    _ when profile.HasEnrolment(AccessTypeCode.SAEforms) => StatusCode.Complete,
                    _ when SAEforms.IsEligible(profile.PartyPlrStanding) => StatusCode.Incomplete,
                    _ => StatusCode.Locked
                };
            }
        }
    }
}
