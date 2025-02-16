namespace FlightReservation.Infrastructure.Email.Services.Models;

public class EmailSettings
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string From { get; set; }
}