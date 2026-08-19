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

    [Fact]
    public async Task CreateOrUpdateRecordAsync_StatusChangesWithoutAffectingGoodStanding_IsStillProcessable()
    {
        // Neither status is in good standing, so this change would have been skipped when
        // ShouldBeProcessed only tracked good-standing flips. Consumers whose eligibility rules
        // are not derived from good standing - e.g. CPS postgrad status, which governs Infant RSV
        // eForms access - never saw it, so a resident whose licence was terminated kept the role.
        var record = this.TestDb.Has(new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = "CPN",
            IdentifierType = "CPSID",
            StatusCode = "PENDING",
            StatusReasonCode = "NONPRAC"
        });
        Assert.False(record.IsGoodStanding);

        var newRecord = new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = "CPN",
            IdentifierType = "CPSID",
            StatusCode = "TERMINATED",
            StatusReasonCode = "NONPRAC"
        };
        Assert.False(newRecord.IsGoodStanding);

        var service = this.MockDependenciesFor<IntakeService>();

        await service.CreateOrUpdateRecordAsync(newRecord, true);

        var log = Assert.Single(this.TestDb.StatusChageLogs.Where(log => log.PlrRecordId == record.Id));
        Assert.True(log.ShouldBeProcessed);
        Assert.Equal("PENDING", log.OldStatusCode);
        Assert.Equal("TERMINATED", log.NewStatusCode);
    }

    [Fact]
    public async Task CreateOrUpdateRecordAsync_StatusUnchanged_WritesNoLog()
    {
        // The guard that keeps the queue from filling with no-op rows.
        var record = this.TestDb.Has(new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = "CPN",
            IdentifierType = "CPSID",
            StatusCode = "ACTIVE",
            StatusReasonCode = "PRAC"
        });

        var service = this.MockDependenciesFor<IntakeService>();

        await service.CreateOrUpdateRecordAsync(new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = "CPN",
            IdentifierType = "CPSID",
            StatusCode = "ACTIVE",
            StatusReasonCode = "PRAC"
        }, true);

        Assert.Empty(this.TestDb.StatusChageLogs.Where(log => log.PlrRecordId == record.Id));
    }
}
