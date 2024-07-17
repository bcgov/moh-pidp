using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpikeWebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/spike-records", async () =>
{
    using var context = new SpikeDbContext();
    var spikeParties = await context.SpikeParty.ToListAsync();
    var findPartyWithEmail = spikeParties
        .Where(p => p.PlrRecord?.RootElement
        .GetProperty("Email")
        .GetString() == "co.lee@gmail.com")
        .FirstOrDefault();

    // var findPartyWithEmail2 = spikeParties
    //     .Where(p => EF.Functions.JsonContains(p.PlrRecord, @"{""firstName"": ""PAM""}"))
    //     .FirstOrDefault();
    var findPartyWithEmail2 = spikeParties
        .Where(p => EF.Functions.JsonExists(p.PlrRecord, "ipc"))
        .FirstOrDefault();
    Console.WriteLine($"{findPartyWithEmail2.FirstName} {findPartyWithEmail2.LastName} has email {findPartyWithEmail2.PlrRecord.RootElement.GetProperty("Email").GetString()}");

    if (findPartyWithEmail?.PlrRecord != null)
    {
        Console.WriteLine("Logging...");
        Console.WriteLine($"{findPartyWithEmail.FirstName} {findPartyWithEmail.LastName} has email {findPartyWithEmail.PlrRecord.RootElement.GetProperty("Email").GetString()}");
        Console.WriteLine($"{findPartyWithEmail.FirstName} {findPartyWithEmail.LastName} has CPN {findPartyWithEmail.PlrRecord.RootElement.GetProperty("cpn").GetString()}");
        Console.WriteLine($"{findPartyWithEmail.FirstName} {findPartyWithEmail.LastName} has licence number {findPartyWithEmail.PlrRecord.RootElement.GetProperty("collegeId").GetInt32()}");
        Console.WriteLine($"{findPartyWithEmail.FirstName} {findPartyWithEmail.LastName} has coordinates length {findPartyWithEmail.PlrRecord.RootElement.GetProperty("coordinates").GetArrayLength()}");
        Console.WriteLine($"{findPartyWithEmail.FirstName} {findPartyWithEmail.LastName} has coordinates {findPartyWithEmail.PlrRecord.RootElement.GetProperty("coordinates")[0]}");

    }
    return Results.Ok(spikeParties);
})
.WithName("GetRecords")
.WithOpenApi();

app.MapPost("/spike-record", async ([FromBody] SpikeParty newSpikeParty) =>
{
    using var context = new SpikeDbContext();
    context.SpikeParty.Add(newSpikeParty);
    await context.SaveChangesAsync();
    return Results.Created($"New SpikeParty created with ID {newSpikeParty.Id}", newSpikeParty);
})
.WithName("PostRecord")
.WithOpenApi();

app.Run();


