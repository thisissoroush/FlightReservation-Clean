using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Enums;
using FlightReservation.Infrastructure.Persistence;
using FlightReservation.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Infrastructure.Test;

public class ReservationRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public ReservationRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ReservationTestDb")
            .Options;
    }

    [Fact]
    public async Task AddAsync_ShouldAddReservation()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new ReservationRepository(context);

        var reservation = new Reservation(1, 1, ReservationStatus.Booked);
        

        // Act
        await repository.ReserveFlight(new CancellationToken(),reservation);
        var savedReservation = await context.Reservations.FirstOrDefaultAsync();

        // Assert
        savedReservation.Should().NotBeNull();
        savedReservation.UserId.Should().Be(1);
    }
}
