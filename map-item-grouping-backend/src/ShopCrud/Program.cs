using MapSolution;

var builder = WebApplication.CreateBuilder(args);

builder.UseOpenTelemetry();

builder.Services.ConfigureMongoDb(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddRoutingInfrastructure();

builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseCors("Cors");

app.MapControllers();

app.Run();