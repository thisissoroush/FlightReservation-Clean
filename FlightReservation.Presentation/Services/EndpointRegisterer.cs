using System.Data.Entity.Infrastructure.Design;
using FlightReservation.Application.Features.Airports;
using FlightReservation.Application.Features.Flights;
using FlightReservation.Application.Features.Reservations;
using FlightReservation.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Presentation.Services;

public static class EndpointRegisterer
{
    public static void RegisterEndPoints(this RouteGroupBuilder api)
    {
        api.RegisterUserEndpoints();
        api.RegisterAirportEndpoints();
        api.RegisterFlightEndpoints();
        api.RegisterReservationEndpoints();
    }

    public static void RegisterUserEndpoints(this IEndpointRouteBuilder api)
    {
        var user = api.MapGroup("/user").WithTags("User");
        
        user.MapPost("/login", async (IMediator mediator, [FromBody] LoginCommand command) =>
        {
            return Results.Ok(await mediator.Send(command));
        });
        
    }
    
    public static void RegisterFlightEndpoints(this IEndpointRouteBuilder api)
    {
        var flight = api.MapGroup("/flight").WithTags("Flight");
        
        flight.MapPost("/create", async (IMediator mediator, [FromBody] CreateFlightCommand command) =>
        {
            await mediator.Send(command);
            return Results.Ok();
        })
        .RequireAuthorization();
        
        
        
        flight.MapPost("/update", async (IMediator mediator, [FromBody] UpdateFlightCommand command) =>
            {
                await mediator.Send(command);
                return Results.Ok();
            })
            .RequireAuthorization();
        
        flight.MapGet("/flights", async (IMediator mediator, [AsParameters] GetFlightsQuery query) =>
        {
            return Results.Ok(await mediator.Send(query));
        });
    }
    
    public static void RegisterAirportEndpoints(this IEndpointRouteBuilder api)
    {
        var airport = api.MapGroup("/airport").WithTags("Airport");
        
        airport.MapPost("/create", async (IMediator mediator, [FromBody] CreateAirportCommand command) =>
        {
            await mediator.Send(command);
            return Results.Ok();
        })
        .RequireAuthorization();
        
        airport.MapGet("/airport", async (IMediator mediator, [AsParameters] GetAirportQuery query) =>
        {
            return Results.Ok(await mediator.Send(query));
        });
        
        airport.MapGet("/airports", async (IMediator mediator, [AsParameters] GetAirportsQuery query) =>
        {
            return Results.Ok(await mediator.Send(query));
        });
        
    }
    
    public static void RegisterReservationEndpoints(this IEndpointRouteBuilder api)
    {
        var reservation = api.MapGroup("/reservation").WithTags("Reservation");
        
        reservation.MapPost("/reserve", async (IMediator mediator, [FromBody] ReserveFlightCommand command) =>
            {
                await mediator.Send(command);
                return Results.Ok();
            })
            .RequireAuthorization();
        
        reservation.MapGet("/get", async (IMediator mediator, [AsParameters] GetReservationsQuery query) =>
        {
            return Results.Ok(await mediator.Send(query));
        });
        
    }
}