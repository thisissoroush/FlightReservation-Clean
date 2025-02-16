using System.ComponentModel.DataAnnotations.Schema;
using FlightReservation.Domain.Enums;

namespace FlightReservation.Domain.Entities;

public class Reservation : AuditableEntity<int>
{
    public Reservation()
    {
        
    }
    public Reservation(int flightId, int userId, ReservationStatus status)
    {
        FlightId = flightId;
        UserId = userId;
        StatusId = (int)status;
    }
    public int FlightId { get; private set; }
    public int UserId { get; private set; }
    public int StatusId { get; private set; } 

    [NotMapped]
    public virtual User User { get; private set; }
    [NotMapped]
    public virtual Flight Flight { get; private set; }

    [NotMapped]
    public ReservationStatus Status => (ReservationStatus)StatusId;

    public void SetUser(User user)
    {
        User = user;
    }

    public void SetFlight(Flight flight)
    {
        Flight = flight;
    }
}