namespace PidpTests.Features.CommonEventHandlers;

using FakeItEasy;
using MassTransit;
using System.Text.Json;
using Xunit;

using Pidp.Features.CommonHandlers;
using static Pidp.Features.CommonHandlers.UpdateKeycloakAttributesConsumer;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class UpdateKeycloakAfterCollegeLicenceUpdatedTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(RecordData))]
    public async void UpdateKeycloakAfterCollegeLicenceUpdated_OneLicence_BusPushedWithMatchingJson(IEnumerable<PlrRecord> expectedRecords)
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.Cpn = "aCpnn";
            party.LicenceDeclaration = new PartyLicenceDeclaration
            {
                LicenceNumber = "12345",
                CollegeCode = CollegeCode.Pharmacists
            };
        });

        List<UpdateKeycloakAttributes> capturedMessages = new();
        var bus = A.Fake<IBus>();
        A.CallTo(() => bus.Publish(A<UpdateKeycloakAttributes>._, A<CancellationToken>._))
            .Invokes(i => capturedMessages.Add(i.GetArgument<UpdateKeycloakAttributes>(0)!));
        var plrClient = A.Fake<IPlrClient>();
        A.CallTo(() => plrClient.GetRecordsAsync(A<string>._)).Returns(expectedRecords);

        var consumer = this.MockDependenciesFor<UpdateKeycloakAfterCollegeLicenceUpdated>(plrClient, bus);

        await consumer.Handle(new CollegeLicenceUpdated(party.Id), new CancellationToken());

        Assert.Single(capturedMessages);

        var message = capturedMessages.Single();
        Assert.Equal(party.PrimaryUserId, message.UserId);
        Assert.Single(message.Attributes);

        var attribute = message.Attributes.Single();
        Assert.Equal("college_licence_info", attribute.Key);
        Assert.Single(attribute.Value);

        var busRecords = JsonSerializer.Deserialize<IEnumerable<PlrRecord>>(attribute.Value[0], new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;

        AssertThat.CollectionsAreEquivalent(expectedRecords, busRecords, (expected, bus) =>
            expected.CollegeId == bus.CollegeId
            && expected.MspId == bus.MspId
            && expected.ProviderRoleType == bus.ProviderRoleType
            && expected.StatusCode == bus.StatusCode
            && expected.StatusReasonCode == bus.StatusReasonCode
        );
    }

    public static IEnumerable<object[]> RecordData()
    {
        yield return new object[] { new[]
        {
            new PlrRecord
            {
                CollegeId = "12345",
                MspId = "Msp123",
                ProviderRoleType = "RoleType",
                StatusCode = "ACTIVE",
                StatusReasonCode = "GS"
            }
        }};

        yield return new object[] { new[]
        {
            new PlrRecord
            {
                CollegeId = "12345",
                MspId = "Msp123",
                ProviderRoleType = "RoleType",
                StatusCode = "ACTIVE",
                StatusReasonCode = "GS"
            },
            new PlrRecord
            {
                CollegeId = "54321",
                MspId = "Msp222",
                ProviderRoleType = "RoleType2",
                StatusCode = "ACTIVE??",
                StatusReasonCode = "GS??"
            }
        }};
    }
}
