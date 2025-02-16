using FlightReservation.Infrastructure.Persistence;
using FlightReservation.Presentation.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Inject();

var app = builder.Build();

var api = app.MapGroup("/api").AddEndpointFilter<EndpointsFilter>();

api.RegisterEndPoints();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

//ef initializer
Initializer.ApplyMigrations(app.Services);

app.Run();