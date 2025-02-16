using FlightReservation.Domain.Entities;

namespace FlightReservation.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUser(CancellationToken ct, int id);
    Task<User> GetUser(CancellationToken ct, string mobile);
    Task<List<User>> GetUsers(CancellationToken ct, int[] ids);

}