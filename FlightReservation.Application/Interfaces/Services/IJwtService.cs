namespace FlightReservation.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(int userId);
    int GetUserId(string token);
}