using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace FlightReservation.Domain.Entities;

public class User : Entity<int>
{
    public User()
    {
        Reservations = new HashSet<Reservation>();
    }

    public string Mobile { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    [NotMapped]
    public virtual ICollection<Reservation> Reservations { get; set; }
}