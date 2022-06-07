namespace PlrIntake.Features.Intake;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using PlrIntake.Data;
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
        this.logger.LogInformation("Add BC Provider message received.");

        var record = new PlrRecordReader(this.logger)
            .ReadDistributionMessage(this.DocumentRoot.ToString());
        var recordId = await this.CreateOrUpdateRecordAsync(record, false);
        this.logger.LogDebug("Id of row created: " + recordId);
    }

    public async Task UpdateBcProviderAsync()
    {
        this.logger.LogInformation("Update BC Provider message received.");

        var record = new PlrRecordReader(this.logger)
            .ReadDistributionMessage(this.DocumentRoot.ToString());
        var recordId = await this.CreateOrUpdateRecordAsync(record, true);
        this.logger.LogDebug("Id of row updated: " + recordId);
    }

    public void LogWarning(string warningMessage) => this.logger.LogWarning(warningMessage);

     private async Task<int> CreateOrUpdateRecordAsync(PlrRecord record, bool expectExists)
    {
        await this.TranslateIdentifierTypeAsync(record);

        var existingRecord = await this.context.PlrRecords
            .SingleOrDefaultAsync(rec => rec.Ipc == record.Ipc);

        if (existingRecord == null)
        {
            this.context.PlrRecords.Add(record);

            if (expectExists)
            {
                this.logger.LogWarning("Expected PLR Provider with IPC of {ipc} to exist but it cannot be found", record.Ipc);
            }
        }
        else
        {
            record.Id = existingRecord.Id;
            this.context.Entry(existingRecord).CurrentValues.SetValues(record);
            existingRecord.Credentials = record.Credentials;
            existingRecord.Expertise = record.Expertise;

            if (!expectExists)
            {
                this.logger.LogWarning("Did not expect PLR Provider with IPC of {ipc} to exist but it was found with ID of {id}", record.Ipc, existingRecord.Id);
            }
        }

        try
        {
            await this.context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error updating PLR Provider with with IPC of {ipc}", record.Ipc);
            return -1;
        }

        return existingRecord == null
            ? record.Id
            : existingRecord.Id;
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
            this.logger.LogError("PLR Provider with IPC of {ipc} had an Identifier OID of {oid} that could not be translated", record.Ipc, record.IdentifierType);
        }
    }
}
