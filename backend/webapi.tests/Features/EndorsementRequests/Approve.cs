namespace PidpTests.Features.EndorsementRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.EndorsementRequests;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;

using PidpTests.TestingExtensions;

public class EndorsementApproveTests : InMemoryDbTest
{
    private static readonly int RequestingPartyId = 1;
    private static readonly int ReceivingPartyId = 2;

    public EndorsementApproveTests()
    {
        this.TestDb.HasAParty(party =>
        {
            party.Id = RequestingPartyId;
            party.FirstName = "Requesting";
            party.LastName = "Party";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email1@email.com";
            party.Phone = "1111234567";
        });
        this.TestDb.HasAParty(party =>
        {
            party.Id = ReceivingPartyId;
            party.FirstName = "Receiving";
            party.LastName = "Party";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email2@email.com";
            party.Phone = "2221234567";
        });
    }

    [Theory]
    [MemberData(nameof(StatusCases))]
    public async void Approve_AsRequestingParty_SuccessOnApproved(EndorsementRequestStatus status)
    {
        var request = this.TestDb.Has(new EndorsementRequest
        {
            RequestingPartyId = RequestingPartyId,
            ReceivingPartyId = ReceivingPartyId,
            Status = status,
            RecipientEmail = "name@example.com"
        });
        var expected = status == EndorsementRequestStatus.Approved; // Requester can only approve ER after approval by reciever.
        var handler = this.MockDependenciesFor<Approve.CommandHandler>();

        var result = await handler.HandleAsync(new Approve.Command { EndorsementRequestId = request.Id, PartyId = RequestingPartyId });

        if (expected)
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(EndorsementRequestStatus.Completed, request.Status);
        }
        else
        {
            Assert.False(result.IsSuccess);
            Assert.Equal(status, request.Status);
        }
    }

    [Theory]
    [MemberData(nameof(StatusCases))]
    public async void Approve_AsRecievingParty_SuccessOnRecieved(EndorsementRequestStatus status)
    {
        var request = this.TestDb.Has(new EndorsementRequest
        {
            RequestingPartyId = RequestingPartyId,
            ReceivingPartyId = ReceivingPartyId,
            Status = status
        });
        var expected = status == EndorsementRequestStatus.Received; // Reciever can only approve ER after recieving.
        var handler = this.MockDependenciesFor<Approve.CommandHandler>();

        var result = await handler.HandleAsync(new Approve.Command { EndorsementRequestId = request.Id, PartyId = ReceivingPartyId });

        if (expected)
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(EndorsementRequestStatus.Approved, request.Status);
        }
        else
        {
            Assert.False(result.IsSuccess);
            Assert.Equal(status, request.Status);
        }
    }

    public static IEnumerable<object[]> StatusCases()
    {
        foreach (var status in TestData.AllValuesOf<EndorsementRequestStatus>())
        {
            yield return new object[] { status };
        }
    }

    [Theory]
    [MemberData(nameof(MoaRoleTestCases))]
    public async void Approve_AsRequester_MoaRoleAssigned(IEnumerable<int> licencedParties, int? expectedRoleAssigned)
    {
        foreach (var partyId in licencedParties)
        {
            var party = this.TestDb.Parties.Single(party => party.Id == partyId);
            party.Cpn = party.FirstName;
        }
        var request = this.TestDb.Has(new EndorsementRequest
        {
            RequestingPartyId = RequestingPartyId,
            ReceivingPartyId = ReceivingPartyId,
            Status = EndorsementRequestStatus.Approved,
            RecipientEmail = "name@example.com"
        });
        var keycloakClient = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var plrClient = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true);
        var handler = this.MockDependenciesFor<Approve.CommandHandler>(keycloakClient, plrClient);

        var result = await handler.HandleAsync(new Approve.Command { EndorsementRequestId = request.Id, PartyId = RequestingPartyId });

        Assert.True(result.IsSuccess);
        if (expectedRoleAssigned == null)
        {
            keycloakClient.AssertNoRolesAssigned();
        }
        else
        {
            var expectedUserId = this.TestDb.Parties
                .Where(party => party.Id == expectedRoleAssigned)
                .Select(party => party.PrimaryUserId)
                .Single();
            A.CallTo(() => keycloakClient.AssignAccessRoles(expectedUserId, MohKeycloakEnrolment.MoaLicenceStatus)).MustHaveHappened();
        }
    }

    public static IEnumerable<object?[]> MoaRoleTestCases()
    {
        yield return new object[] { Enumerable.Empty<int>(), null! }; // neither are licenced, no role assigned.
        yield return new object[] { new[] { RequestingPartyId }, ReceivingPartyId };
        yield return new object[] { new[] { ReceivingPartyId }, RequestingPartyId };
        yield return new object[] { new[] { RequestingPartyId, ReceivingPartyId }, null! }; // Both are licenced, no role assigned
    }
}
