namespace Pidp.Features.Parties;

using NodaTime;

using Pidp.Models.Lookups;

public partial class ProfileStatus
{
    public partial class Model
    {
        public class AccessAdministrator : ProfileSection
        {
            internal override string SectionName => "administratorInfo";
            public string? Email { get; set; }

            public AccessAdministrator(ProfileStatusDto profile) : base(profile) => this.Email = profile.AccessAdministratorEmail;

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
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

        public class CollegeCertification : ProfileSection
        {
            internal override string SectionName => "collegeCertification";
            public bool HasCpn { get; set; }
            public bool LicenceDeclared { get; set; }

            public CollegeCertification(ProfileStatusDto profile) : base(profile)
            {
                this.HasCpn = !string.IsNullOrWhiteSpace(profile.Cpn);
                this.LicenceDeclared = profile.HasDeclaredLicence;
            }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
                if (!profile.UserIsBcServicesCard)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (!profile.DemographicsEntered)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                if (profile.LicenceDeclaration == null)
                {
                    this.StatusCode = StatusCode.Incomplete;
                    return;
                }

                if (profile.LicenceDeclaration.HasNoLicence
                    || profile.PlrStanding.HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (profile.PlrStanding.Error)
                {
                    this.Alerts.Add(Alert.TransientError);
                    this.StatusCode = StatusCode.Error;
                    return;
                }

                this.Alerts.Add(Alert.PlrBadStanding);
                this.StatusCode = StatusCode.Error;
            }
        }

        public class Demographics : ProfileSection
        {
            internal override string SectionName => "demographics";
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public LocalDate? Birthdate { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }

            public Demographics(ProfileStatusDto profile) : base(profile)
            {
                this.FirstName = profile.FirstName;
                this.LastName = profile.LastName;
                this.Birthdate = profile.Birthdate;
                this.Email = profile.Email;
                this.Phone = profile.Phone;
            }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile) => this.StatusCode = profile.DemographicsEntered ? StatusCode.Complete : StatusCode.Incomplete;
        }

        public class OrganizationDetails : ProfileSection
        {
            internal override string SectionName => "organizationDetails";

            public OrganizationDetails(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
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

        public class DriverFitness : ProfileSection
        {
            internal override string SectionName => "driverFitness";

            public DriverFitness(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
                if (!profile.UserIsBcServicesCard)
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
                    || !profile.PlrStanding.HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                this.StatusCode = StatusCode.Incomplete;
            }
        }

        public class HcimAccountTransfer : ProfileSection
        {
            internal override string SectionName => "hcimAccountTransfer";

            public HcimAccountTransfer(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
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

        public class HcimEnrolment : ProfileSection
        {
            internal override string SectionName => "hcimEnrolment";

            public HcimEnrolment(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
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

        public class MSTeams : ProfileSection
        {
            internal override string SectionName => "msTeams";

            public MSTeams(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
                if (!profile.UserIsBcServicesCard)
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
                    || !profile.PlrStanding
                        .With(AccessRequests.MSTeams.AllowedIdentifierTypes)
                        .HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                this.StatusCode = StatusCode.Incomplete;
            }
        }

        public class SAEforms : ProfileSection
        {
            internal override string SectionName => "saEforms";

            public bool IncorrectLicenceType { get; set; }

            public SAEforms(ProfileStatusDto profile) : base(profile)
            {
                this.IncorrectLicenceType = profile.PlrStanding.HasGoodStanding
                    && !profile.PlrStanding
                        .Excluding(AccessRequests.SAEforms.ExcludedIdentifierTypes)
                        .HasGoodStanding;
            }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
                if (!profile.UserIsBcServicesCard)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.SAEforms))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (!profile.DemographicsEntered
                    || !profile.PlrStanding
                        .Excluding(AccessRequests.SAEforms.ExcludedIdentifierTypes)
                        .HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                this.StatusCode = StatusCode.Incomplete;
            }
        }

        public class Uci : ProfileSection
        {
            internal override string SectionName => "uci";

            public Uci(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
                if (!profile.UserIsBcServicesCard)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessTypeCode.Uci))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (!profile.DemographicsEntered
                    || !profile.PlrStanding.HasGoodStanding)
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                this.StatusCode = StatusCode.Incomplete;
            }
        }
    }
}
