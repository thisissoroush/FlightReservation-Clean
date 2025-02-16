using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using FlightReservation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Infrastructure.Repositories;

public class AirportRepository: IAirportRepository
{
    private readonly ApplicationDbContext _dbContext;
    public AirportRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Airport> GetAirport(CancellationToken ct, string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new FlightReservationException(400,"Airport code cannot be empty");

        return await _dbContext.Airports.AsNoTracking()
            .Where(a => a.Code.Equals(code.Trim()))
            .FirstOrDefaultAsync(ct);

    }

    public async Task<Airport> GetAirport(CancellationToken ct, int id)
    {
        if (id.Equals(default))
            throw new FlightReservationException(400,"Wrong airport id");

        return await _dbContext.Airports.AsNoTracking()
            .Where(a => a.Id.Equals(id))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<Airport>> GetAirports(CancellationToken ct, string? code, string? name, string? city, string? country)
    {
        var query = _dbContext.Airports.AsNoTracking();

        if (!string.IsNullOrEmpty(code))
            query = query.Where(a => a.Code.Equals(code.Trim()));
        
        if (!string.IsNullOrEmpty(name))
            query = query.Where(a => a.Name.Equals(name.Trim()));
        
        if (!string.IsNullOrEmpty(city))
            query = query.Where(a => a.City.Equals(city.Trim()));
        
        if (!string.IsNullOrEmpty(country))
            query = query.Where(a => a.Country.Equals(country.Trim()));
        
        return await query.ToListAsync(ct);
    }

    public async Task AddAirport(CancellationToken ct, Airport airport)
    {
        await _dbContext.Airports.AddAsync(airport,ct);
        await _dbContext.SaveChangesAsync(ct);
    }
}