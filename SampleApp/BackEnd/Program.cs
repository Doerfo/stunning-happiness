using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // current workaround for port forwarding in codespaces
    // https://github.com/dotnet/aspnetcore/issues/57332
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Servers = [];
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/definitaly-no-security-flaw", (HttpContext ctx) =>
{
    // Intentionally vulnerable code for CodeQL testing (command injection).
    var cmd = ctx.Request.Query["cmd"].ToString();
    if (string.IsNullOrWhiteSpace(cmd))
        return Results.BadRequest("Provide ?cmd=<command>");

    var psi = new ProcessStartInfo
    {
        FileName = "/bin/sh",
        ArgumentList = { "-c", cmd }, // CodeQL should flag this.
        RedirectStandardOutput = true
    };
    var proc = Process.Start(psi);
    proc!.WaitForExit(2000);
    var output = proc.StandardOutput.ReadToEnd();
    return Results.Ok(output);
})
.WithName("definitaly-no-security-flaw");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
