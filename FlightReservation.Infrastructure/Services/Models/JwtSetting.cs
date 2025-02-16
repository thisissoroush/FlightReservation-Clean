namespace FlightReservation.Infrastructure.Email.Services.Models;

public class JwtSetting
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
}