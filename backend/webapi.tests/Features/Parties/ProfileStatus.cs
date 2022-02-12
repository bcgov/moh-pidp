namespace PidpTests.Features.Parties;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Models;
using Pidp.Models.Lookups;
using Pidp.Features.Parties.ProfileStatusInternal;
using Pidp.Infrastructure.HttpClients.Plr;
using static Pidp.Features.Parties.ProfileStatus;
using static Pidp.Features.Parties.ProfileStatus.Model;

public class ProfileStatusTests : InMemoryDbTest
{
    [Fact]
    public async void HandleAsync_NoProfile_NoCompleteSections()
    {
        var party = this.TestDb.Has(new Party { FirstName = "first", LastName = "last", Birthdate = LocalDate.FromDateTime(DateTime.Today) });
        var client = A.Fake<IPlrClient>();
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert>(), profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Incomplete, StatusCode.Locked, StatusCode.Locked);
    }

    [Fact]
    public async void HandleAsync_DemographicsFinished_OneCompleteSection()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "first",
            LastName = "last",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "an@email.com",
            Phone = "5555555555"
        });
        var client = A.Fake<IPlrClient>();
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert>(), profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Incomplete, StatusCode.Locked);

        var demographics = profile.Demographics();
        Assert.Equal(party.FirstName, demographics.FirstName);
        Assert.Equal(party.LastName, demographics.LastName);
        Assert.Equal(party.Birthdate, demographics.Birthdate);
        Assert.Equal(party.Email, demographics.Email);
        Assert.Equal(party.Phone, demographics.Phone);
    }

    [Fact]
    public async void HandleAsync_CertificationFinishedWithIpcAndGoodStanding_TwoCompleteSections()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "first",
            LastName = "last",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "an@email.com",
            Phone = "5555555555",
            PartyCertification = new PartyCertification
            {
                CollegeCode = CollegeCode.Pharmacists,
                LicenceNumber = "12345",
                Ipc = "IPC"
            }
        });
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns(new MockedPlrRecordStatus(true));
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert>(), profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Complete, StatusCode.Incomplete);

        var certification = profile.Certification();
        Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
        Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
    }

    [Fact]
    public async void HandleAsync_CertificationFinishedWithIpcAndBadStanding_Error()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "first",
            LastName = "last",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "an@email.com",
            Phone = "5555555555",
            PartyCertification = new PartyCertification
            {
                CollegeCode = CollegeCode.Pharmacists,
                LicenceNumber = "12345",
                Ipc = "IPC"
            }
        });
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns(new MockedPlrRecordStatus(false));
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert> { Alert.PlrBadStanding }, profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Error, StatusCode.Locked);

        var certification = profile.Certification();
        Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
        Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
    }

    [Fact]
    public async void HandleAsync_CertificationFinishedWithIpcAndRecordStatusCannotBeFound_TransientError()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "first",
            LastName = "last",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "an@email.com",
            Phone = "5555555555",
            PartyCertification = new PartyCertification
            {
                CollegeCode = CollegeCode.Pharmacists,
                LicenceNumber = "12345",
                Ipc = "IPC"
            }
        });
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns((PlrRecordStatus)null!);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert> { Alert.TransientError }, profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Error, StatusCode.Locked);

        var certification = profile.Certification();
        Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
        Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
    }

    [Fact]
    public async void HandleAsync_CertificationFinishedWithNoIpcCannotBeFound_TransientError()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "first",
            LastName = "last",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "an@email.com",
            Phone = "5555555555",
            PartyCertification = new PartyCertification
            {
                CollegeCode = CollegeCode.Pharmacists,
                LicenceNumber = "12345",
                Ipc = null
            }
        });
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate)).Returns((string)null!);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        Assert.Null(party.PartyCertification!.Ipc);
        A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate)).MustHaveHappenedOnceExactly();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert> { Alert.TransientError }, profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Error, StatusCode.Locked);

        var certification = profile.Certification();
        Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
        Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
    }

    [Fact]
    public async void HandleAsync_CertificationFinishedWithNoIpcButThenFoundInGoodStanding_IpcUpdatedTwoCompleteSections()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "first",
            LastName = "last",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "an@email.com",
            Phone = "5555555555",
            PartyCertification = new PartyCertification
            {
                CollegeCode = CollegeCode.Pharmacists,
                LicenceNumber = "12345",
                Ipc = null
            }
        });
        var expectedIpc = "newIPC";
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate)).Returns(expectedIpc);
        A.CallTo(() => client.GetRecordStatus(expectedIpc)).Returns(new MockedPlrRecordStatus(true));
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedIpc, party.PartyCertification!.Ipc);
        A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate)).MustHaveHappenedOnceExactly();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert>(), profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Complete, StatusCode.Incomplete);

        var certification = profile.Certification();
        Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
        Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
    }

    [Fact]
    public async void HandleAsync_CompletedProfileAndSaEnrolement_EverythingComplete()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "first",
            LastName = "last",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "an@email.com",
            Phone = "5555555555",
            PartyCertification = new PartyCertification
            {
                CollegeCode = CollegeCode.Pharmacists,
                LicenceNumber = "12345",
                Ipc = "IPC"
            },
            AccessRequests = new[]
            {
                new AccessRequest
                {
                    AccessType = AccessType.SAEforms
                }
            }
        });
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns(new MockedPlrRecordStatus(true));
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(new Command { Id = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();

        var profile = result.Value;
        Assert.Equal(new HashSet<Alert>(), profile.Alerts);
        profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Complete, StatusCode.Complete);
    }

    private class MockedPlrRecordStatus : PlrRecordStatus
    {
        private readonly bool goodStanding;
        public MockedPlrRecordStatus(bool goodStanding) => this.goodStanding = goodStanding;

        public override bool IsGoodStanding() => this.goodStanding;
    }
}

public static class ProfileStatusExtensions
{
    public static void AssertSectionStatus(this Model profileStatus, StatusCode demographics, StatusCode certs, StatusCode saEforms)
    {
        Assert.Equal(demographics, profileStatus.Status[Section.Demographics].StatusCode);
        Assert.Equal(certs, profileStatus.Status[Section.CollegeCertification].StatusCode);
        Assert.Equal(saEforms, profileStatus.Status[Section.SAEforms].StatusCode);
    }

    public static DemographicsSection Demographics(this Model profileStatus) => (DemographicsSection)profileStatus.Status[Section.Demographics];
    public static CollegeCertificationSection Certification(this Model profileStatus) => (CollegeCertificationSection)profileStatus.Status[Section.CollegeCertification];
}
