namespace PidpTests.Features.EndorsementRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using static Pidp.Features.EndorsementRequests.Index;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;


public class EndorsementRequestIndexTests : InMemoryDbTest
{
    [Fact]
    public async void Index_AsRequesterCreated_InList()
    {
        var party = this.TestDb.HasAParty();
        var request = this.TestDb.Has(new EndorsementRequest
        {
            RequestingPartyId = party.Id,
            RecipientEmail = "anemail@email.com",
            AdditionalInformation = "info",
            Status = EndorsementRequestStatus.Created,
            StatusDate = Instant.FromDateTimeOffset(DateTimeOffset.Now)
        });
        var handler = this.MockDependenciesFor<QueryHandler>();

        var result = await handler.HandleAsync(new Query { PartyId = party.Id });

        Assert.Single(result);
        var model = result.Single();
        Assert.Equal(request.Id, model.Id);
        Assert.Equal(request.RecipientEmail, model.RecipientEmail);
        Assert.Equal(request.AdditionalInformation, model.AdditionalInformation);
        Assert.Null(model.PartyName);
        Assert.Null(model.CollegeCode);
        Assert.Equal(request.Status, model.Status);
        Assert.Equal(request.StatusDate, model.StatusDate);
        Assert.False(model.Actionable);
    }

    [Theory]
    [MemberData(nameof(StatusCases))]
    public async void Index_AsRequesterOrReciever_InList(EndorsementRequestStatus status)
    {
        var party = this.TestDb.HasAParty();
        var partyRecieving = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "recFirst";
            party.FirstName = "recLast";
            party.LicenceDeclaration = new PartyLicenceDeclaration
            {
                CollegeCode = CollegeCode.NursesAndMidwives
            };
        });
        var partyRequesting = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "reqFirst";
            party.FirstName = "reqLast";
            party.LicenceDeclaration = new PartyLicenceDeclaration
            {
                CollegeCode = CollegeCode.NaturopathicPhysicians
            };
        });
        var sentRequest = this.TestDb.Has(new EndorsementRequest
        {
            RequestingPartyId = party.Id,
            ReceivingPartyId = partyRecieving.Id,
            RecipientEmail = "an1email@email.com",
            AdditionalInformation = "info1",
            Status = status,
            StatusDate = Instant.FromDateTimeOffset(DateTimeOffset.Now)
        });
        var recievedRequest = this.TestDb.Has(new EndorsementRequest
        {
            RequestingPartyId = partyRequesting.Id,
            ReceivingPartyId = party.Id,
            RecipientEmail = "an2email@email.com",
            AdditionalInformation = "info2",
            Status = status,
            StatusDate = Instant.FromDateTimeOffset(DateTimeOffset.Now)
        });
        var handler = this.MockDependenciesFor<QueryHandler>();

        var results = await handler.HandleAsync(new Query { PartyId = party.Id });

        if (status is EndorsementRequestStatus.Completed or EndorsementRequestStatus.Expired)
        {
            Assert.Empty(results);
            return;
        }

        Assert.Equal(2, results.Count);
        Assert.True(results.All(result => result != null));

        var sentResult = results.Single(result => result.Id == sentRequest.Id);
        Assert.Equal(sentRequest.RecipientEmail, sentResult.RecipientEmail);
        Assert.Equal(sentRequest.AdditionalInformation, sentResult.AdditionalInformation);
        Assert.Equal(partyRecieving.FullName, sentResult.PartyName);
        Assert.Equal(partyRecieving.LicenceDeclaration!.CollegeCode, sentResult.CollegeCode);
        Assert.Equal(sentRequest.Status, sentResult.Status);
        Assert.Equal(sentRequest.StatusDate, sentResult.StatusDate);
        Assert.Equal(sentRequest.Status == EndorsementRequestStatus.Approved, sentResult.Actionable);

        var recievedResult = results.Single(result => result.Id == recievedRequest.Id);
        Assert.Equal(recievedRequest.RecipientEmail, recievedResult.RecipientEmail);
        Assert.Equal(recievedRequest.AdditionalInformation, recievedResult.AdditionalInformation);
        Assert.Equal(partyRequesting.FullName, recievedResult.PartyName);
        Assert.Equal(partyRequesting.LicenceDeclaration!.CollegeCode, recievedResult.CollegeCode);
        Assert.Equal(recievedRequest.Status, recievedResult.Status);
        Assert.Equal(recievedRequest.StatusDate, recievedResult.StatusDate);
        Assert.Equal(recievedRequest.Status == EndorsementRequestStatus.Received, recievedResult.Actionable);
    }

    public static IEnumerable<object[]> StatusCases()
    {
        foreach (var status in TestData.AllValuesOf<EndorsementRequestStatus>().Where(status => status != EndorsementRequestStatus.Created))
        {
            yield return new object[] { status };
        }
    }
}
