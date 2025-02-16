using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ReservationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ReserveFlight(CancellationToken ct, Reservation reservation)
    {
        await _dbContext.AddAsync(reservation, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<List<Reservation>> GetReservations(CancellationToken ct,
        int? flightId = null,
        int? userId = null)
    {
        var query = _dbContext.Reservations.AsNoTracking();

        if (flightId is not null && flightId > default(int))
            query = query.Where(q => q.FlightId.Equals(flightId));

        if (userId is not null && userId > default(int))
            query = query.Where(q => q.UserId.Equals(userId));

        return await query.ToListAsync(ct);
    }
}