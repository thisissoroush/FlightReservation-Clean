using FlightReservation.Domain.Entities;

namespace FlightReservation.Application.Interfaces.Repositories;

public interface IAirportRepository
{
    Task<Airport> GetAirport(CancellationToken ct, string code);
    Task<Airport> GetAirport(CancellationToken ct, int id);
    Task<List<Airport>> GetAirports(CancellationToken ct, string? code, string? name, string? city, string? country);
    Task AddAirport(CancellationToken ct,Airport airport);
}