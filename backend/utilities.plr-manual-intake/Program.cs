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
    /// Can be used like this: `dotnet run PLR_Test_Data_IAT20210617_v2.0.csv intake.log`
    /// </summary>
    /// <param name="args">Expecting path to .csv file and desired log file</param>
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(args[1])
            .CreateLogger();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var connectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()
            .GetConnectionString("PlrDatabase");

        var optionsBuilder = new DbContextOptionsBuilder<PlrDbContext>()
            .UseNpgsql(connectionString);

        Log.Information($"Started loading {args[0]} at {DateTime.Now}");
        try
        {
            var dbContext = new PlrDbContext(optionsBuilder.Options);

            using var stream = new StreamReader(args[0]);
            using var reader = new CsvReader(stream, CultureInfo.InvariantCulture);
            var numProviders = 0;
            var rowNum = 1;

            // Consume header row
            reader.Read();

            while (reader.Read())
            {
                try
                {
                    rowNum++;
                    var provider = PlrRecordParser.ReadRow(reader);
                    PlrRecordParser.CheckData(provider, rowNum);
                    dbContext.PlrRecords.Add(provider);
                    try
                    {
                        dbContext.SaveChanges();
                        numProviders++;
                    }
                    catch (DbUpdateException e)
                    {
                        // e.g. May get `duplicate key value violates unique constraint "IX_PlrProvider_Ipc"` error
                        Log.Error(e, $"Error saving {nameof(PlrRecord)} at row number {rowNum} to the database.");
                        dbContext.PlrRecords.Remove(provider);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, $"Error ingesting row at row number {rowNum}, IPC: {reader.GetField<string>(0)}");
                }
            }

            Log.Information($"Number of providers loaded: {numProviders}, Final row number: {rowNum}.");
        }
        catch (Exception e)
        {
            Log.Error(e, "Unexpected error encountered.");
        }
        finally
        {
            Log.Information($"Program ended at {DateTime.Now}");
            Console.WriteLine($"Program completed ... check {args[1]}.");
        }
    }
}
