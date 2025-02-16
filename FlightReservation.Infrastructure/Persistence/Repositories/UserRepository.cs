using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using FlightReservation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> GetUser(CancellationToken ct, int id)
    {
        if (id.Equals(default))
            throw new FlightReservationException(400,"User id is invalid.");

        return await _dbContext.Users.AsNoTracking()
            .Where(u => u.Id.Equals(id))
            .FirstOrDefaultAsync(ct);
    

    }

    public async Task<User> GetUser(CancellationToken ct, string mobile)
    {
        if (string.IsNullOrEmpty(mobile))
            throw new FlightReservationException(400,"mobile number id is invalid.");

        return await _dbContext.Users.AsNoTracking()
            .Where(u => u.Mobile.Equals(mobile.Trim()))
            .FirstOrDefaultAsync(ct);

        
    }

    public async Task<List<User>> GetUsers(CancellationToken ct, int[] ids)
    {
        if (!ids.Any())
            throw new FlightReservationException(400,"no user id provided.");

        return await _dbContext.Users.AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(ct);
    }
}