using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Flights;


public record UpdateFlightCommand(
    int Id,
    DateTime DepartureDate,
    DateTime ArrivalDate,
    decimal Price,
    int AvailableSeats) : DefaultRequestParams, IRequest;

// Features/Flights/UpdateFlightHandler.cs
public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand>
{
    private readonly IFlightRepository _flightRepository; 
    public UpdateFlightCommandHandler(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }
    public async Task Handle(UpdateFlightCommand request, CancellationToken ct)
    {
        var flight = await _flightRepository.GetFlight(ct, request.Id);

        if (flight is null)
            throw new FlightReservationException(400,"Flight with this number already exists");
       
        flight.UpdateFlightDates(request.DepartureDate, request.ArrivalDate);
        flight.UpdatePrice(request.Price);
        flight.UpdateSeats(request.AvailableSeats);
        
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