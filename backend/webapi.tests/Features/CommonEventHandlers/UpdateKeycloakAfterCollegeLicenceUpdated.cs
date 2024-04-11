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
    [Fact]
    public async void UpdateKeycloakAfterCollegeLicenceUpdated_OneLicence_BusPushedWithMatchingJson()
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
        var expectedRecord = new PlrRecord
        {
            CollegeId = "12345",
            MspId = "Msp123",
            ProviderRoleType = "RoleType",
            StatusCode = "ACTIVE",
            StatusReasonCode = "GS"
        };

        List<UpdateKeycloakAttributes> capturedMessages = new();
        var bus = A.Fake<IBus>();
        A.CallTo(() => bus.Publish(A<UpdateKeycloakAttributes>._, A<CancellationToken>._))
            .Invokes(i => capturedMessages.Add(i.GetArgument<UpdateKeycloakAttributes>(0)!));
        var plrClient = A.Fake<IPlrClient>();
        A.CallTo(() => plrClient.GetRecordsAsync(A<string>._)).Returns(new[] { expectedRecord });

        var consumer = this.MockDependenciesFor<UpdateKeycloakAfterCollegeLicenceUpdated>(plrClient, bus);

        await consumer.Handle(new CollegeLicenceUpdated(party.Id), new CancellationToken());

        Assert.Single(capturedMessages);

        var message = capturedMessages.Single();
        Assert.Equal(party.PrimaryUserId, message.UserId);
        Assert.Single(message.Attributes);

        var attribute = message.Attributes.Single();
        Assert.Equal("college_licence_info", attribute.Key);
        Assert.Single(attribute.Value);

        var attributeValue = JsonSerializer.Deserialize<PlrRecord>(attribute.Value[0], new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
        Assert.Equal(expectedRecord.CollegeId, attributeValue.CollegeId);
        Assert.Equal(expectedRecord.MspId, attributeValue.MspId);
        Assert.Equal(expectedRecord.ProviderRoleType, attributeValue.ProviderRoleType);
        Assert.Equal(expectedRecord.StatusCode, attributeValue.StatusCode);
        Assert.Equal(expectedRecord.StatusReasonCode, attributeValue.StatusReasonCode);
    }
}
