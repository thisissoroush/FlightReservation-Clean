using FlightReservation.Domain.Entities;

namespace FlightReservation.Application.Interfaces.Repositories;

public interface IFlightRepository
{
    Task<Flight> GetFlight(CancellationToken ct, string flightNumber);
    Task<Flight> GetFlight(CancellationToken ct, int id);
    
    Task<List<Flight>> GetFlights(CancellationToken ct, int sourceAirportId, int destinationAirportId,  DateTime? startDate = null,
        DateTime? endDate = null, bool? onlyAvailableSeats = false);

    Task<List<Flight>> GetFlights(CancellationToken ct, int[] ids);
    Task AddFlight(CancellationToken ct, Flight flight);
    Task UpdateFlight(CancellationToken ct, Flight flight);
    
}