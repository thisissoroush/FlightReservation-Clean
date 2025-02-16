
using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using FlightReservation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Infrastructure.Repositories;

public class FlightRepository: IFlightRepository
{
    private readonly ApplicationDbContext _dbContext;
    public FlightRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Flight> GetFlight(CancellationToken ct, string flightNumber)
    {
        return await _dbContext.Flights.AsNoTracking()
            .Where(f => f.FlightNumber.Equals(flightNumber.Trim()))
            .FirstOrDefaultAsync(ct);

    }

    public async Task<Flight> GetFlight(CancellationToken ct, int id)
    {
        return await _dbContext.Flights.AsNoTracking()
            .Where(f => f.Id.Equals(id))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<Flight>> GetFlights(CancellationToken ct,
        int sourceAirportId,
        int destinationAirportId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        bool? onlyAvailableSeats = false)
    {
        var query = _dbContext.Flights.AsNoTracking()
            .Where(f => f.SourceAirportId.Equals(sourceAirportId) && destinationAirportId.Equals(f.DestinationAirportId));

        
        if (onlyAvailableSeats ?? false)
            query = query.Where(q => q.AvailableSeats > 0);
        
        if (startDate is not null)
            query = query.Where(q => q.DepartureDate >= startDate );
        
        if (endDate is not null )
            query = query.Where(q => q.DepartureDate <= endDate );

       
        return await query.ToListAsync(ct);

    }

    public async Task<List<Flight>> GetFlights(CancellationToken ct, int[] ids)
    {
        if (!ids.Any())
            throw new FlightReservationException(400, "No id provided");

        return await _dbContext.Flights.AsNoTracking()
            .Where(f => ids.Contains(f.Id))
            .ToListAsync(ct);
    }

    public async Task AddFlight(CancellationToken ct, Flight flight)
    {
        await _dbContext.Flights.AddAsync(flight,ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateFlight(CancellationToken ct, Flight flight)
    {
        _dbContext.Flights.Update(flight);
        await _dbContext.SaveChangesAsync(ct);
    }
}