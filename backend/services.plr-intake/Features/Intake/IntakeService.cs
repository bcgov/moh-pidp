namespace PlrIntake.Features.Intake;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ServiceModel;
using System.Xml.Linq;

using PlrIntake.Data;
using PlrIntake.Features.Intake.RecordReader;
using PlrIntake.Models;

[ServiceContract(Namespace = IntakeServiceOperationTuner.Uri)]
public interface IIntakeService
{
    [OperationContract(Name = "PRPM_IN301030CA")]
    Task AddBcProviderAsync();

    [OperationContract(Name = "PRPM_IN303030CA")]
    Task UpdateBcProviderAsync();
}

public class IntakeService : IIntakeService
{
    private readonly ILogger<IntakeService> logger;
    private readonly PlrDbContext context;

    public IntakeService(
        ILogger<IntakeService> logger,
        PlrDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    //We expect that the DocumentRoot will be set in the IntakeServiceOperationTuner.
    public XElement DocumentRoot { get; set; } = default!;

    public async Task AddBcProviderAsync()
    {
        this.logger.LogAddMessageRecieved();

        var record = new PlrRecordReader(this.logger)
            .ReadDistributionMessage(this.DocumentRoot.ToString());
        var recordId = await this.CreateOrUpdateRecordAsync(record, false);

        this.logger.LogRecordCreated(recordId);
    }

    public async Task UpdateBcProviderAsync()
    {
        this.logger.LogUpdateMessageRecieved();

        var record = new PlrRecordReader(this.logger)
            .ReadDistributionMessage(this.DocumentRoot.ToString());
        var recordId = await this.CreateOrUpdateRecordAsync(record, true);

        this.logger.LogRecordUpdated(recordId);
    }

    public void LogUnrecognizedCert(string requestCertThumbprint) => this.logger.LogUnrecognizedCert(requestCertThumbprint);

    public async Task<int> CreateOrUpdateRecordAsync(PlrRecord record, bool expectExists)
    {
        await this.TranslateIdentifierTypeAsync(record);

        var existingRecord = await this.context.PlrRecords
            .SingleOrDefaultAsync(rec => rec.Ipc == record.Ipc);

        if (existingRecord == null)
        {
            this.context.PlrRecords.Add(record);
            this.CheckStatusChange(null, record);

            if (expectExists)
            {
                this.logger.LogIpcNotFound(record.Ipc);
            }
        }
        else
        {
            this.CheckStatusChange(existingRecord, record);

            record.Id = existingRecord.Id;
            this.context.Entry(existingRecord).CurrentValues.SetValues(record);
            existingRecord.Credentials = record.Credentials;
            existingRecord.Expertise = record.Expertise;

            if (!expectExists)
            {
                this.logger.LogUnexpectedIpcFound(record.Ipc, existingRecord.Id);
            }
        }

        try
        {
            await this.context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            this.logger.LogDatabaseException(e, record.Ipc);
            return -1;
        }

        return record.Id;
    }

    private async Task TranslateIdentifierTypeAsync(PlrRecord record)
    {
        var identifierType = await this.context.IdentifierTypes
            .SingleOrDefaultAsync(identifier => identifier.Oid == record.IdentifierType);

        if (identifierType != null)
        {
            // Translate from "2.16.840.1.113883.3.40.2.20" to "RNPID", for example
            record.IdentifierType = identifierType.Name;
        }
        else
        {
            this.logger.LogUntranslatableIpc(record.Ipc, record.IdentifierType);
        }
    }

    private void CheckStatusChange(PlrRecord? existingRecord, PlrRecord newRecord)
    {
        if (existingRecord == null
            || existingRecord.StatusCode != newRecord.StatusCode
            || existingRecord.StatusReasonCode != newRecord.StatusReasonCode)
        {
            this.context.StatusChageLogs.Add(new StatusChageLog
            {
                PlrRecord = newRecord,
                OldStatusCode = existingRecord?.StatusCode,
                OldStatusReasonCode = existingRecord?.StatusReasonCode,
                NewStatusCode = newRecord.StatusCode,
                NewStatusReasonCode = newRecord.StatusReasonCode,
                ShouldBeProcessed = existingRecord?.IsGoodStanding != newRecord.IsGoodStanding
            });
        }
    }
}

public static partial class IntakeServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "A client provided an unrecognized certifcate with thumbprint {requestCertThumbprint}.")]
    public static partial void LogUnrecognizedCert(this ILogger logger, string requestCertThumbprint);

    [LoggerMessage(2, LogLevel.Information, "Add BC Provider message received.")]
    public static partial void LogAddMessageRecieved(this ILogger logger);

    [LoggerMessage(3, LogLevel.Debug, "Id of row created: {recordId}")]
    public static partial void LogRecordCreated(this ILogger logger, int recordId);

    [LoggerMessage(4, LogLevel.Information, "Update BC Provider message received.")]
    public static partial void LogUpdateMessageRecieved(this ILogger logger);

    [LoggerMessage(5, LogLevel.Debug, "Id of row updated: {recordId}")]
    public static partial void LogRecordUpdated(this ILogger logger, int recordId);

    [LoggerMessage(6, LogLevel.Warning, "Expected PLR Provider with IPC of {ipc} to exist but it cannot be found")]
    public static partial void LogIpcNotFound(this ILogger logger, string ipc);

    [LoggerMessage(7, LogLevel.Warning, "Did not expect PLR Provider with IPC of {ipc} to exist but it was found with ID of {existingRecordId}")]
    public static partial void LogUnexpectedIpcFound(this ILogger logger, string ipc, int existingRecordId);

    [LoggerMessage(8, LogLevel.Error, "Error updating PLR Provider with with IPC of {ipc}")]
    public static partial void LogDatabaseException(this ILogger logger, Exception e, string ipc);

    [LoggerMessage(9, LogLevel.Error, "PLR Provider with IPC of {ipc} had an Identifier OID of {oid} that could not be translated")]
    public static partial void LogUntranslatableIpc(this ILogger logger, string ipc, string? oid);
}
