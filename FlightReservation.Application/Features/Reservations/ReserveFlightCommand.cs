using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Application.Interfaces.Services;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Enums;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Reservations;

public record ReserveFlightCommand(int FlightId, int? customerId, bool? includeWaitList) : DefaultRequestParams,IRequest;

public class ReserveFlightCommandHandler : IRequestHandler<ReserveFlightCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IEmailService _emailService;
    
    public ReserveFlightCommandHandler(
        IReservationRepository reservationRepository, 
        IUserRepository userRepository, 
        IFlightRepository flightRepository, IEmailService emailService)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _flightRepository = flightRepository;
        _emailService = emailService;
    }
    public async Task Handle(ReserveFlightCommand request, CancellationToken ct)
    {
        var currentUser = await _userRepository.GetUser(ct, request.UserId);

        User customer = new();
        if (request.customerId.HasValue)
        {
            if(!(currentUser?.IsAdmin??false))
                throw new FlightReservationException(400,
                    "reserving flights for other users is only available by admins.");

            customer = await _userRepository.GetUser(ct, request.customerId.Value);

            if (customer is null)
                throw new FlightReservationException(400,
                    "No user found with the specified id.");
        }
        else
            customer = currentUser;
           
        
        var flight = await _flightRepository.GetFlight(ct, request.FlightId);

        if (flight == null)
            throw new FlightReservationException(400, "flight not found.");
        
        ReservationStatus status = ReservationStatus.Booked;

        if (flight.AvailableSeats.Equals(default(int)))
        {
            if (!(request.includeWaitList ?? false))
                throw new FlightReservationException(400, "This flight has no available seats.");

            status = ReservationStatus.Waitlisted;
        }
        
        try
        {
            var reservation = new Reservation(
                request.FlightId,
                customer.Id,
                status
                );

            await _reservationRepository.ReserveFlight(ct, reservation);
            
            Task.Run(() => _emailService.SendEmailAsync(customer.Email,
                "Succesful Reservation", 
                $@"
                    Dear {customer.FirstName} {customer.LastName},
                    Flight {flight.FlightNumber} was successfully {status.ToString()} for you.
                "));
        }
        catch (Exception ex)
        {
            throw new FlightReservationException(500, "An Unhandled error occured during reservation process.");
        }
        
    }
}