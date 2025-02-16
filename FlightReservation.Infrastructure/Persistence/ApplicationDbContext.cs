using FlightReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
  

    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Airport> Airports { get; set; } = null!;
    public virtual DbSet<Flight> Flights { get; set; } = null!;
    public virtual DbSet<Reservation> Reservations { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=FlightReservation;Trusted_Connection=True;");
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            
            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.HasIndex(e => e.Email, "NCI_Customer_Email");

            
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            
            entity.HasIndex(e => e.Mobile, "NCI_Customer_Mobile");
         

            entity.Property(e => e.Password).HasMaxLength(100);
            
            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
               

            entity.Property(e => e.FirstName).HasMaxLength(50);

            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.ToTable("Flight");
            
            
            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(e => e.FlightNumber).HasMaxLength(50);
            
            entity.HasIndex(e => e.SourceAirportId, "NCI_Flight_SourceAirportId");
            entity.HasIndex(e => e.DestinationAirportId, "NCI_Flight_DestinationAirportId");

            entity.Property(e => e.DepartureDate);
            entity.Property(e => e.ArrivalDate);
            
            entity.Property(e => e.SourceAirportId);
            entity.Property(e => e.DestinationAirportId);
            entity.Property(e => e.Price).HasColumnType("decimal(20, 3)");
            entity.Property(e => e.AvailableSeats);
            
            entity.Property(e => e.CreateDateUtc).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastModifyDateUtc);
            
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.SourceAirport)
                .WithMany() // assuming no back reference on Airport
                .HasForeignKey(f => f.SourceAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestinationAirport)
                .WithMany() 
                .HasForeignKey(f => f.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("Reservation");
            
            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            
            entity.Property(e => e.FlightId);
            entity.Property(e => e.UserId);
            entity.Property(e => e.StatusId);
            
            
            entity.HasIndex(e => e.FlightId, "NCI_Reservation_FlightId");
            
            entity.HasIndex(e => e.UserId, "NCI_Reservation_UserId");
            
            entity.Property(e => e.CreateDateUtc).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastModifyDateUtc);
            
            entity.HasOne<Flight>()
                .WithMany(p=> p.Reservations)
                .HasForeignKey(e => e.FlightId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne<User>()
                .WithMany(p=> p.Reservations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        });
        
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.ToTable("Airport");
            
            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            
            entity.Property(e => e.CreateDateUtc).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastModifyDateUtc);
        });
        
        SeedPreDefined(modelBuilder);        
    }


    private void SeedPreDefined(ModelBuilder modelBuilder)
    {
        // Predefined users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@flightreservation.com",
                Mobile = "09121234567",
                Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", // Use a hashed password in real-world scenarios
                IsAdmin = true,
                FirstName = "Admin",
                LastName = "User"
            },
            new User
            {
                Id = 2,
                Email = "customer@flightreservation.com",
                Mobile = "09381234567",
                Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", // Use a hashed password in real-world scenarios
                IsAdmin = false,
                FirstName = "Customer",
                LastName = "User"
            }
        );
    }
}
