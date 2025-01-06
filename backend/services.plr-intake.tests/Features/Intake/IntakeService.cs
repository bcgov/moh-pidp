namespace PlrIntakeTests.Features.Intake;

using PlrIntake.Features.Intake;
using PlrIntake.Models;
using PlrIntakeTests.TestingExtensions;
using Xunit;


public class IntakeServiceTests : InMemoryDbTest
{
    [Fact]
    public async Task CreateOrUpdateRecordAsync_RecordDoesNotExist_AddsRecord()
    {
        // Arrange
        var cpn = "CPN";
        var record = new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = cpn,
            IdentifierType = "CPSID",
            CollegeId = "12345",
            ProviderRoleType = "ProviderRoleType",
            StatusCode = "StatusCode",
            StatusStartDate = DateTime.Today,
            StatusReasonCode = "StatusReasonCode"
        };

        var service = this.MockDependenciesFor<IntakeService>();

        // Act
        var result = await service.CreateOrUpdateRecordAsync(record, false);

        // Assert
        Assert.Equal(record.Id, result);
        Assert.Single(this.TestDb.StatusChageLogs
            .Where(log => log.PlrRecordId == record.Id && log.ShouldBeProcessed));
    }

    [Fact]
    public async Task CreateOrUpdateRecordAsync_RecordExists_UpdatesRecord()
    {
        // Arrange
        var cpn = "CPN";
        var record = this.TestDb.Has(new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = cpn,
            IdentifierType = "CPSID",
            CollegeId = "12345",
            ProviderRoleType = "ProviderRoleType",
            StatusCode = "StatusCode",
            StatusStartDate = DateTime.Today - TimeSpan.FromDays(7),
            StatusReasonCode = "StatusReasonCode"
        });

        var newRecord = new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = cpn,
            IdentifierType = "CPSID",
            CollegeId = "12345",
            ProviderRoleType = "ProviderRoleType",
            StatusCode = "BadStatusCode",
            StatusStartDate = DateTime.Today,
            StatusReasonCode = "BadStatusReasonCode"
        };

        var service = this.MockDependenciesFor<IntakeService>();

        // Act
        var result = await service.CreateOrUpdateRecordAsync(newRecord, true);

        // Assert
        Assert.Equal(record.Id, result);
    }
}
