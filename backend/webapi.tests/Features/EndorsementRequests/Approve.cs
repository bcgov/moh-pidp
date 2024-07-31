namespace PidpTests.Features.EndorsementRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.EndorsementRequests;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
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
    public async Task Approve_AsRequestingParty_SuccessOnApproved(EndorsementRequestStatus status)
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
    public async Task Approve_AsRecievingParty_SuccessOnRecieved(EndorsementRequestStatus status)
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

    [Theory]
    [MemberData(nameof(MoaRoleTestCases))]
    public async Task Approve_AsRequester_MoaRoleAssigned(IEnumerable<int> licencedParties, int? expectedRoleAssigned)
    {
        foreach (var partyId in licencedParties)
        {
            var party = this.TestDb.Parties.Single(party => party.Id == partyId);
            party.Cpn = "cpn";
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
            .ReturningAStandingsDigestWhenCalledWithCpn("cpn", true);
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

    [Fact]
    public async Task Approve_AsReceivingParty_Send_Email_to_RequestingParty()
    {
        var request = this.TestDb.Has(new EndorsementRequest
        {
            RequestingPartyId = RequestingPartyId,
            ReceivingPartyId = ReceivingPartyId,
            Status = EndorsementRequestStatus.Received,
            RecipientEmail = "Email1@email.com"
        });

        var emailService = AMock.EmailService();
        var handler = this.MockDependenciesFor<Approve.CommandHandler>(emailService);

        var result = await handler.HandleAsync(new Approve.Command { EndorsementRequestId = request.Id, PartyId = ReceivingPartyId });
        Assert.True(result.IsSuccess);
        A.CallTo(() => emailService.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();
    }

    public static TheoryData<EndorsementRequestStatus> StatusCases => new(TestData.AllValuesOf<EndorsementRequestStatus>());

    public static TheoryData<int[], int?> MoaRoleTestCases => new()
    {
        // { [ Licenced PartyIds ], PartyIds expected to have MOA role assigned }
        { [], null }, // neither are licenced, no role assigned.
        { [ RequestingPartyId ], ReceivingPartyId },
        { [ ReceivingPartyId ], RequestingPartyId },
        { [ RequestingPartyId, ReceivingPartyId ], null }, // Both are licenced, no role assigned
    };
}
