using FlightReservation.Domain.Entities;

namespace FlightReservation.Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task ReserveFlight(CancellationToken ct, Reservation reservation);
    
    Task<List<Reservation>> GetReservations(CancellationToken ct ,
        int? flightId = null,
        int? userId = null);
    
}