// namespace PidpTests.Features.Parties;

// using FakeItEasy;
// using NodaTime;
// using System.Security.Claims;
// using Xunit;

// using Pidp.Extensions;
// using Pidp.Models;
// using Pidp.Models.Lookups;
// using Pidp.Infrastructure.HttpClients.Plr;
// using static Pidp.Features.Parties.ProfileStatus;
// using static Pidp.Features.Parties.ProfileStatus.Model;
// using PidpTests.TestingExtensions;
// using Pidp.Infrastructure.Auth;

// public class ProfileStatusTests : InMemoryDbTest
// {
//     [Fact]
//     public async void HandleAsync_BcscNoProfile_NoCompleteSections()
//     {
//         var party = this.TestDb.Has(new Party { FirstName = "first", LastName = "last", Birthdate = LocalDate.FromDateTime(DateTime.Today) });
//         var client = A.Fake<IPlrClient>();
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();
//         Assert.Equal(new HashSet<Alert>(), profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Incomplete, StatusCode.Locked, StatusCode.Hidden, StatusCode.Locked);
//     }

//     [Fact]
//     public async void HandleAsync_BcscDemographicsFinished_OneCompleteSection()
//     {
//         var party = this.TestDb.Has(new Party
//         {
//             FirstName = "first",
//             LastName = "last",
//             Birthdate = LocalDate.FromDateTime(DateTime.Today),
//             Email = "an@email.com",
//             Phone = "5555555555"
//         });
//         var client = A.Fake<IPlrClient>();
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();
//         Assert.Equal(new HashSet<Alert>(), profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Incomplete, StatusCode.Hidden, StatusCode.Locked);

//         var demographics = profile.Section<Demographics>();
//         Assert.Equal(party.FirstName, demographics.FirstName);
//         Assert.Equal(party.LastName, demographics.LastName);
//         Assert.Equal(party.Birthdate, demographics.Birthdate);
//         Assert.Equal(party.Email, demographics.Email);
//         Assert.Equal(party.Phone, demographics.Phone);
//     }

//     [Fact]
//     public async void HandleAsync_BcscCertificationFinishedWithIpcAndGoodStanding_TwoCompleteSections()
//     {
//         var party = this.TestDb.Has(new Party
//         {
//             FirstName = "first",
//             LastName = "last",
//             Birthdate = LocalDate.FromDateTime(DateTime.Today),
//             Email = "an@email.com",
//             Phone = "5555555555",
//             PartyCertification = new PartyCertification
//             {
//                 CollegeCode = CollegeCode.Pharmacists,
//                 LicenceNumber = "12345",
//                 Ipc = "IPC"
//             }
//         });
//         var client = A.Fake<IPlrClient>();
//         A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns(new MockedPlrRecordStatus(true));
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();
//         Assert.Equal(new HashSet<Alert>(), profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Complete, StatusCode.Hidden, StatusCode.Incomplete);

//         var certification = profile.Section<CollegeCertification>();
//         Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
//         Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
//     }

//     [Fact]
//     public async void HandleAsync_BcscCertificationFinishedWithIpcAndBadStanding_Error()
//     {
//         var party = this.TestDb.Has(new Party
//         {
//             FirstName = "first",
//             LastName = "last",
//             Birthdate = LocalDate.FromDateTime(DateTime.Today),
//             Email = "an@email.com",
//             Phone = "5555555555",
//             PartyCertification = new PartyCertification
//             {
//                 CollegeCode = CollegeCode.Pharmacists,
//                 LicenceNumber = "12345",
//                 Ipc = "IPC"
//             }
//         });
//         var client = A.Fake<IPlrClient>();
//         A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns(new MockedPlrRecordStatus(false));
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();
//         Assert.Equal(new HashSet<Alert> { Alert.PlrBadStanding }, profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Error, StatusCode.Hidden, StatusCode.Locked);

//         var certification = profile.Section<CollegeCertification>();
//         Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
//         Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
//     }

//     [Fact]
//     public async void HandleAsync_BcscCertificationFinishedWithIpcAndRecordStatusCannotBeFound_TransientError()
//     {
//         var party = this.TestDb.Has(new Party
//         {
//             FirstName = "first",
//             LastName = "last",
//             Birthdate = LocalDate.FromDateTime(DateTime.Today),
//             Email = "an@email.com",
//             Phone = "5555555555",
//             PartyCertification = new PartyCertification
//             {
//                 CollegeCode = CollegeCode.Pharmacists,
//                 LicenceNumber = "12345",
//                 Ipc = "IPC"
//             }
//         });
//         var client = A.Fake<IPlrClient>();
//         A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns((PlrRecordStatus)null!);
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();
//         Assert.Equal(new HashSet<Alert> { Alert.TransientError }, profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Error, StatusCode.Hidden, StatusCode.Locked);

//         var certification = profile.Section<CollegeCertification>();
//         Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
//         Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
//     }

//     [Fact]
//     public async void HandleAsync_BcscCertificationFinishedWithNoIpcCannotBeFound_TransientError()
//     {
//         var party = this.TestDb.Has(new Party
//         {
//             FirstName = "first",
//             LastName = "last",
//             Birthdate = LocalDate.FromDateTime(DateTime.Today),
//             Email = "an@email.com",
//             Phone = "5555555555",
//             PartyCertification = new PartyCertification
//             {
//                 CollegeCode = CollegeCode.Pharmacists,
//                 LicenceNumber = "12345",
//                 Ipc = null
//             }
//         });
//         var client = A.Fake<IPlrClient>();
//         A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate!.Value)).Returns((string)null!);
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         Assert.Null(party.PartyCertification!.Ipc);
//         A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate!.Value)).MustHaveHappenedOnceExactly();
//         Assert.Equal(new HashSet<Alert> { Alert.TransientError }, profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Error, StatusCode.Hidden, StatusCode.Locked);

//         var certification = profile.Section<CollegeCertification>();
//         Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
//         Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
//     }

//     [Fact]
//     public async void HandleAsync_BcscCertificationFinishedWithNoIpcButThenFoundInGoodStanding_IpcUpdatedTwoCompleteSections()
//     {
//         var party = this.TestDb.Has(new Party
//         {
//             FirstName = "first",
//             LastName = "last",
//             Birthdate = LocalDate.FromDateTime(DateTime.Today),
//             Email = "an@email.com",
//             Phone = "5555555555",
//             PartyCertification = new PartyCertification
//             {
//                 CollegeCode = CollegeCode.Pharmacists,
//                 LicenceNumber = "12345",
//                 Ipc = null
//             }
//         });
//         var expectedIpc = "newIPC";
//         var client = A.Fake<IPlrClient>();
//         A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate!.Value)).Returns(expectedIpc);
//         A.CallTo(() => client.GetRecordStatus(expectedIpc)).Returns(new MockedPlrRecordStatus(true));
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         Assert.Equal(expectedIpc, party.PartyCertification!.Ipc);
//         A.CallTo(() => client.GetPlrRecord(party.PartyCertification!.CollegeCode, party.PartyCertification.LicenceNumber, party.Birthdate!.Value)).MustHaveHappenedOnceExactly();
//         Assert.Equal(new HashSet<Alert>(), profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Complete, StatusCode.Hidden, StatusCode.Incomplete);

//         var certification = profile.Section<CollegeCertification>();
//         Assert.Equal(party.PartyCertification!.CollegeCode, certification.CollegeCode);
//         Assert.Equal(party.PartyCertification!.LicenceNumber, certification.LicenceNumber);
//     }

//     [Fact]
//     public async void HandleAsync_BcscCompletedProfileAndSaEnrolement_EverythingComplete()
//     {
//         var party = this.TestDb.Has(new Party
//         {
//             FirstName = "first",
//             LastName = "last",
//             Birthdate = LocalDate.FromDateTime(DateTime.Today),
//             Email = "an@email.com",
//             Phone = "5555555555",
//             PartyCertification = new PartyCertification
//             {
//                 CollegeCode = CollegeCode.Pharmacists,
//                 LicenceNumber = "12345",
//                 Ipc = "IPC"
//             },
//             AccessRequests = new[]
//             {
//                 new AccessRequest
//                 {
//                     AccessTypeCode = AccessTypeCode.SAEforms
//                 }
//             }
//         });
//         var client = A.Fake<IPlrClient>();
//         A.CallTo(() => client.GetRecordStatus(party.PartyCertification!.Ipc)).Returns(new MockedPlrRecordStatus(true));
//         var handler = this.MockDependenciesFor<CommandHandler>(client);

//         var profile = await handler.HandleAsync(new Command { Id = party.Id, User = MockedBcscUser() });

//         A.CallTo(() => client.GetPlrRecord(A<CollegeCode>._, A<string>._, A<LocalDate>._)).MustNotHaveHappened();
//         Assert.Equal(new HashSet<Alert>(), profile.Alerts);
//         profile.AssertSectionStatus(StatusCode.Complete, StatusCode.Complete, StatusCode.Hidden, StatusCode.Complete);
//     }

//     // TODO HCIM tests

//     private class MockedPlrRecordStatus : PlrRecordStatus
//     {
//         private readonly bool goodStanding;
//         public MockedPlrRecordStatus(bool goodStanding) => this.goodStanding = goodStanding;

//         public override bool IsGoodStanding() => this.goodStanding;
//     }

//     private static ClaimsPrincipal MockedBcscUser()
//     {
//         var user = A.Fake<ClaimsPrincipal>();
//         A.CallTo(() => user.FindFirst(Claims.IdentityProvider)).Returns(new Claim(Claims.IdentityProvider, ClaimValues.BCServicesCard));
//         return user;
//     }

//     private static ClaimsPrincipal MockedPhsaUser()
//     {
//         var user = A.Fake<ClaimsPrincipal>();
//         A.CallTo(() => user.FindFirst(Claims.IdentityProvider)).Returns(new Claim(Claims.IdentityProvider, ClaimValues.Phsa));
//         return user;
//     }
// }

// public static class ProfileStatusExtensions
// {
//     public static void AssertSectionStatus(this Model profileStatus, StatusCode demographics, StatusCode collegeCertification, StatusCode hcimAccountTransfer, StatusCode saEformsStatus)
//     {
//         Assert.Equal(demographics, profileStatus.Section<Demographics>().StatusCode);
//         Assert.Equal(collegeCertification, profileStatus.Section<CollegeCertification>().StatusCode);
//         Assert.Equal(hcimAccountTransfer, profileStatus.Section<Model.HcimAccountTransfer>().StatusCode);
//         Assert.Equal(saEformsStatus, profileStatus.Section<SAEforms>().StatusCode);
//     }

//     public static T Section<T>(this Model profileStatus) where T : ProfileSection => profileStatus.Status.Values.OfType<T>().Single();
// }
