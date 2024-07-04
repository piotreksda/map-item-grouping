using MapSolution;

var builder = WebApplication.CreateBuilder(args);

builder.UseOpenTelemetry();

var app = builder.Build();

app.Run();