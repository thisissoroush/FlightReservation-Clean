using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Flights;

public record CreateFlightCommand(
    string FlightNumber,
    DateTime DepartureDate,
    DateTime ArrivalDate,
    int SourceAirportId,
    int DestinationAirportId,
    decimal Price,
    int AvailableSeats) : DefaultRequestParams,IRequest;

// Features/Flights/CreateFlightHandler.cs
public class CreateFlightHandler : IRequestHandler<CreateFlightCommand>
{
    private readonly IFlightRepository _flightRepository; 
    public CreateFlightHandler(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }
    public async Task Handle(CreateFlightCommand request, CancellationToken ct)
    {
        var existingFlight = await _flightRepository.GetFlight(ct, request.FlightNumber);

       if (existingFlight is not null)
           throw new FlightReservationException(400,"Flight with this number already exists");
       
       var flight = new Flight(
           request.FlightNumber,
           request.DepartureDate,
           request.ArrivalDate,
           request.SourceAirportId,
           request.DestinationAirportId,
           request.Price,
           request.AvailableSeats);

       try
       {
            await _flightRepository.AddFlight(ct, flight);
       }
       catch (Exception ex)
       {
           throw new FlightReservationException(500, "An unhandled error occured.",ex.Message);
       }
    }
}