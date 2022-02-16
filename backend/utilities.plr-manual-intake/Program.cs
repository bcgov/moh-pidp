namespace PlrManualIntake;

using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Globalization;

using PlrIntake.Models;
using PlrIntake.Data;

public class Program
{
    /// <summary>
    /// Can be used like this: `dotnet run PLR_Test_Data_IAT20210617_v2.0.csv`
    /// </summary>
    /// <param name="args">Expecting path to .csv file</param>
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(config["LogFile"])
            .CreateLogger();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var dbOptions = new DbContextOptionsBuilder<PlrDbContext>()
            .UseNpgsql(config.GetConnectionString("PlrDatabase"))
            .Options;

        Log.Information($"Started loading {args[0]} at {DateTime.Now}");
        try
        {
            var dbContext = CreateContext(dbOptions);
            var batchSize = 1000;
            var recordCount = 0;
            var rowNum = 1;

            using var stream = new StreamReader(args[0]);
            using var reader = new CsvReader(stream, CultureInfo.InvariantCulture);

            // Consume header row
            reader.Read();

            while (reader.Read())
            {
                rowNum++;

                PlrRecord? record = null;
                try
                {
                    record = PlrRecordParser.ReadRow(reader);
                }
                catch (Exception e)
                {
                    Log.Error(e, $"Error parsing row at row number {rowNum}");
                    continue;
                }

                if (record == null)
                {
                    Log.Error($"Null Record at row number {rowNum}");
                    continue;
                }

                recordCount++;
                PlrRecordParser.CheckData(record, rowNum);
                dbContext = AddToContext(dbContext, record, recordCount, batchSize, dbOptions);
            }

            dbContext.SaveChanges();

            Log.Information($"Number of records loaded: {recordCount}, Final row number: {rowNum}.");
        }
        catch (Exception e)
        {
            Log.Error(e, "Unexpected error encountered.");
        }
        finally
        {
            Log.Information($"Program ended at {DateTime.Now}");
            Console.WriteLine($"Program completed ... check log file.");
        }
    }

    private static PlrDbContext AddToContext(PlrDbContext context, PlrRecord record, int count, int batchSize, DbContextOptions<PlrDbContext>? recreationOptions = null)
    {
        context.PlrRecords.Add(record);

        if (count % batchSize == 0)
        {
            context.SaveChanges();

            if (recreationOptions != null)
            {
                context.Dispose();
                context = CreateContext(recreationOptions);
            }
        }

        return context;
    }

    private static PlrDbContext CreateContext(DbContextOptions<PlrDbContext> options)
    {
        var context = new PlrDbContext(options);
        context.ChangeTracker.AutoDetectChangesEnabled = false;
        return context;
    }
}
