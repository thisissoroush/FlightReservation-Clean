using System.ComponentModel.DataAnnotations.Schema;

namespace FlightReservation.Domain.Entities;

public class Flight : AuditableEntity<int>
{

    public Flight()
    {
        
    }
    public Flight(string flightNumber, DateTime departureDate, DateTime arrivalDate, int sourceAirportId, int destinationAirportId, decimal price, int availableSeats)
    {
        FlightNumber = flightNumber;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        SourceAirportId = sourceAirportId;
        DestinationAirportId = destinationAirportId;
        Price = price;
        AvailableSeats = availableSeats;
    }
    public string FlightNumber { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public DateTime ArrivalDate { get; private set; }
    public int SourceAirportId { get; private set; }
    public int DestinationAirportId { get; private set; }
    public decimal Price { get; private set; }
    public int AvailableSeats { get; private set; }
    
    
    [NotMapped]
    public virtual Airport SourceAirport { get; set; } = null!;
    [NotMapped]
    public virtual Airport DestinationAirport { get; set; } = null!;
    [NotMapped]
    public virtual ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();

    public void UpdateFlightDates(DateTime departureDate, DateTime arrivalDate)
    {
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        UpdateLastModifyDateUtc();
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
        UpdateLastModifyDateUtc();
    }

    public void UpdateSeats(int availableSeats)
    {
        AvailableSeats = availableSeats;
        UpdateLastModifyDateUtc();
    }


}