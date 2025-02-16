using FlightReservation.Application.Features.Reservations;
using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Enums;
using Moq;
using Xunit;
using FluentAssertions;

public class ReserveFlightCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IFlightRepository> _flightRepositoryMock;
    private readonly ReserveFlightCommandHandler _handler;

    public ReserveFlightCommandHandlerTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _flightRepositoryMock = new Mock<IFlightRepository>();

        _handler = new ReserveFlightCommandHandler(
            _reservationRepositoryMock.Object,
            _userRepositoryMock.Object,
            _flightRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateReservation_WhenFlightExists()
    {
        // Arrange
        var command = new ReserveFlightCommand(1, null, false)
        {
            UserId = 1
        };

        var currentUser = new User
        {
            Id = 1,
            IsAdmin = false
        };

        var flight = new Flight("123",
            DateTime.Now.AddHours(4),
            DateTime.Now.AddDays(1),
            1,
            1,
            2300,
            10);
       

        _userRepositoryMock
            .Setup(repo => repo.GetUser(It.IsAny<CancellationToken>(), command.UserId))
            .ReturnsAsync(currentUser);

        _flightRepositoryMock
            .Setup(repo => repo.GetFlight(It.IsAny<CancellationToken>(), command.FlightId))
            .ReturnsAsync(flight);

        _reservationRepositoryMock
            .Setup(repo => repo.ReserveFlight(It.IsAny<CancellationToken>(), It.IsAny<Reservation>()))
            .Returns(Task.CompletedTask);

        // Act
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().NotThrowAsync();

        _reservationRepositoryMock.Verify(repo => 
            repo.ReserveFlight(
                It.IsAny<CancellationToken>(), 
                It.Is<Reservation>(r => 
                    r.FlightId == command.FlightId && 
                    r.UserId == currentUser.Id && 
                    r.Status == ReservationStatus.Booked
                )
            ), 
            Times.Once
        );
    }
}
