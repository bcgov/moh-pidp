namespace Pidp.Features.Parties;

using NodaTime;
using Pidp.Features.AccessRequests;
using Pidp.Models.Lookups;

public partial class ProfileStatus
{
    public partial class Model
    {
        public enum Alert
        {
            TransientError = 1,
            PlrBadStanding
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

            protected abstract void Compute(ProfileData profile);

            public static TSection Create<TSection>(ProfileData profile) where TSection : ProfileSection, new()
            {
                var section = new TSection();
                section.Compute(profile);
                return section;
            }
        }

        public class DashboardInfoSection : ProfileSection
        {
            internal override string SectionName => "dashboardInfo";
            public string FullName { get; set; } = string.Empty;
            public CollegeCode? CollegeCode { get; set; }

            protected override void Compute(ProfileData profile)
            {
                this.FullName = $"{profile.FirstName} {profile.LastName}";
                this.CollegeCode = profile.LicenceDeclaration?.CollegeCode;
                this.StatusCode = StatusCode.Complete; // Unused
            }
        }

        public class AccessAdministratorSection : ProfileSection
        {
            internal override string SectionName => "administratorInfo";
            public string? Email { get; set; }

            protected override void Compute(ProfileData profile)
            {
                this.Email = profile.AccessAdministratorEmail;

                if (!profile.UserIsPhsa)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                this.StatusCode = string.IsNullOrWhiteSpace(profile.AccessAdministratorEmail)
                    ? StatusCode.Incomplete
                    : StatusCode.Complete;
            }
        }

        public class CollegeCertificationSection : ProfileSection
        {
            internal override string SectionName => "collegeCertification";
            public bool HasCpn { get; set; }
            public bool LicenceDeclared { get; set; }

            protected override void Compute(ProfileData profile)
            {
                this.HasCpn = !string.IsNullOrWhiteSpace(profile.Cpn);
                this.LicenceDeclared = profile.CollegeLicenceDeclared;

                if (!profile.UserHasHighAssuranceIdentity)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (!profile.DemographicsEntered)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                if (!profile.LicenceDeclarationEntered)
                {
                    this.StatusCode = StatusCode.Incomplete;
                    return;
                }

                if (profile.LicenceDeclaration.HasNoLicence
                    || profile.PartyPlrStanding.HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (profile.PartyPlrStanding.Error)
                {
                    this.Alerts.Add(Alert.TransientError);
                    this.StatusCode = StatusCode.Error;
                    return;
                }

                this.Alerts.Add(Alert.PlrBadStanding);
                this.StatusCode = StatusCode.Error;
            }
        }

        public class DemographicsSection : ProfileSection
        {
            internal override string SectionName => "demographics";
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public LocalDate? Birthdate { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }

            protected override void Compute(ProfileData profile)
            {
                this.FirstName = profile.FirstName;
                this.LastName = profile.LastName;
                this.Birthdate = profile.Birthdate;
                this.Email = profile.Email;
                this.Phone = profile.Phone;

                this.StatusCode = profile.DemographicsEntered ? StatusCode.Complete : StatusCode.Incomplete;
            }
        }

        public class OrganizationDetailsSection : ProfileSection
        {
            internal override string SectionName => "organizationDetails";

            protected override void Compute(ProfileData profile)
            {
                if (!profile.UserIsPhsa)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (!profile.DemographicsEntered)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                this.StatusCode = profile.OrganizationDetailEntered
                    ? StatusCode.Complete
                    : StatusCode.Incomplete;
            }
        }

        public class DriverFitnessSection : ProfileSection
        {
            internal override string SectionName => "driverFitness";

            protected override void Compute(ProfileData profile)
            {
                if (!profile.UserHasHighAssuranceIdentity)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.DriverFitness))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (!profile.DemographicsEntered
                    || !profile.LicenceDeclarationEntered)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                if (profile.PartyPlrStanding
                    .With(DriverFitness.AllowedIdentifierTypes)
                    .HasGoodStanding
                    || (profile.LicenceDeclaration.HasNoLicence && profile.EndorsementPlrStanding.HasGoodStanding)) // TODO: We should defer this calcualtion until this step if possible
                {
                    this.StatusCode = StatusCode.Incomplete;
                    return;
                }

                this.StatusCode = StatusCode.Locked;
            }
        }

        public class HcimAccountTransferSection : ProfileSection
        {
            internal override string SectionName => "hcimAccountTransfer";

            protected override void Compute(ProfileData profile)
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
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimEnrolment))
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }
                // ]

                this.StatusCode = profile.DemographicsEntered
                    ? StatusCode.Incomplete
                    : StatusCode.Locked;
            }
        }

        public class HcimEnrolmentSection : ProfileSection
        {
            internal override string SectionName => "hcimEnrolment";

            protected override void Compute(ProfileData profile)
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
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.HcimEnrolment))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }
                // ]

                this.StatusCode = !profile.DemographicsEntered || string.IsNullOrWhiteSpace(profile.AccessAdministratorEmail)
                    ? StatusCode.Locked
                    : StatusCode.Incomplete;
            }
        }

        public class MSTeamsSection : ProfileSection
        {
            internal override string SectionName => "msTeams";

            protected override void Compute(ProfileData profile)
            {
                if (!profile.UserHasHighAssuranceIdentity)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.MSTeams))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (!profile.DemographicsEntered
                    || !profile.PartyPlrStanding
                        .With(MSTeams.AllowedIdentifierTypes)
                        .HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                this.StatusCode = StatusCode.Incomplete;
            }
        }

        public class PrescriptionRefillEformsSection : ProfileSection
        {
            internal override string SectionName => "prescriptionRefillEforms";

            protected override void Compute(ProfileData profile)
            {
                if (!profile.UserHasHighAssuranceIdentity)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.PrescriptionRefillEforms))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (profile.DemographicsEntered
                    && profile.PartyPlrStanding
                        .With(PrescriptionRefillEforms.AllowedIdentifierTypes)
                        .HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Incomplete;
                    return;
                }

                this.StatusCode = StatusCode.Locked;
            }
        }

        public class SAEformsSection : ProfileSection
        {
            internal override string SectionName => "saEforms";
            public bool IncorrectLicenceType { get; set; }

            protected override void Compute(ProfileData profile)
            {
                this.IncorrectLicenceType = profile.PartyPlrStanding.HasGoodStanding
                    && !profile.PartyPlrStanding
                        .Excluding(SAEforms.ExcludedIdentifierTypes)
                        .HasGoodStanding;

                if (!profile.UserHasHighAssuranceIdentity)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.SAEforms))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (profile.DemographicsEntered
                    && profile.PartyPlrStanding
                        .Excluding(SAEforms.ExcludedIdentifierTypes)
                        .HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Incomplete;
                    return;
                }

                this.StatusCode = StatusCode.Locked;
            }
        }

    }
}
