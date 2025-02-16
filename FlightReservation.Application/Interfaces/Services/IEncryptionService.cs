namespace FlightReservation.Application.Interfaces.Services;

public interface IEncryptionService
{
    string Encrypt(string password);
}