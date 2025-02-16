namespace FlightReservation.Domain.Entities;

public class Airport : AuditableEntity<int>
{
    public Airport()
    {
        
    }
    public Airport(string code, string name, string city, string country)
    {
        Code = code;
        Name = name;
        City = city;
        Country = country;
    }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
}