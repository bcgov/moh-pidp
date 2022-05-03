namespace Pidp.Features.Parties;

using NodaTime;

using Pidp.Models;
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

            protected override void SetAlertsAndStatus(ProfileStatusDto profile) => this.StatusCode = string.IsNullOrWhiteSpace(profile.AccessAdministratorEmail) ? StatusCode.Incomplete : StatusCode.Complete;
        }

        public class CollegeCertification : ProfileSection
        {
            internal override string SectionName => "collegeCertification";
            public CollegeCode? CollegeCode { get; set; }
            public string? LicenceNumber { get; set; }

            public CollegeCertification(ProfileStatusDto profile) : base(profile)
            {
                this.CollegeCode = profile.CollegeCode;
                this.LicenceNumber = profile.LicenceNumber;
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

                if (!profile.CollegeCertificationEntered)
                {
                    this.StatusCode = StatusCode.Incomplete;
                    return;
                }

                if (profile.Ipc == null
                    || profile.PlrRecordStatus == null)
                {
                    this.Alerts.Add(Alert.TransientError);
                    this.StatusCode = StatusCode.Error;
                    return;
                }

                if (!profile.PlrRecordStatus.IsGoodStanding())
                {
                    this.Alerts.Add(Alert.PlrBadStanding);
                    this.StatusCode = StatusCode.Error;
                    return;
                }

                this.StatusCode = StatusCode.Complete;
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

        public class HcimAccountTransfer : ProfileSection
        {
            internal override string SectionName => "hcimAccountTransfer";

            public HcimAccountTransfer(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
                if (profile.UserIsBcServicesCard)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessType.HcimAccountTransfer))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

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
                if (profile.CompletedEnrolments.Contains(AccessType.HcimEnrolment))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                this.StatusCode = !profile.DemographicsEntered || string.IsNullOrWhiteSpace(profile.AccessAdministratorEmail)
                    ? StatusCode.Locked
                    : StatusCode.Incomplete;
            }
        }

        public class SAEforms : ProfileSection
        {
            internal override string SectionName => "saEforms";

            public SAEforms(ProfileStatusDto profile) : base(profile) { }

            protected override void SetAlertsAndStatus(ProfileStatusDto profile)
            {
                if (!profile.UserIsBcServicesCard)
                {
                    this.StatusCode = StatusCode.Hidden;
                    return;
                }

                if (profile.CompletedEnrolments.Contains(AccessType.SAEforms))
                {
                    this.StatusCode = StatusCode.Complete;
                    return;
                }

                if (!profile.DemographicsEntered
                    || !profile.CollegeCertificationEntered
                    || profile.PlrRecordStatus == null
                    || !profile.PlrRecordStatus.IsGoodStanding())
                {
                    this.StatusCode = StatusCode.Locked;
                    return;
                }

                this.StatusCode = StatusCode.Incomplete;
            }
        }
    }
}
