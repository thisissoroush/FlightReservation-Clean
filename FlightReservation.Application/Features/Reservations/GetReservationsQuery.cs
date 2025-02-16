using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Reservations;

public record GetReservationsQuery(int? flightId, int? customerId)
    : DefaultRequestParams, IRequest<GetReservationsQueryResponse>;

public record GetReservationsQueryResponse(List<Reservation> reservations);

public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, GetReservationsQueryResponse>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFlightRepository _flightRepository;

    public GetReservationsQueryHandler(IReservationRepository reservationRepository,
        IUserRepository userRepository,
        IFlightRepository flightRepository)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _flightRepository = flightRepository;
    }

    public async Task<GetReservationsQueryResponse> Handle(GetReservationsQuery request, CancellationToken ct)
    {
        var currentUser = await _userRepository.GetUser(ct, request.UserId);

        //Admin checking the customers reservations
        if (request.customerId is not null)
        {
            if (request.customerId.Equals(default))
                throw new FlightReservationException(400,
                    "invalid customer id.");

            if (!currentUser.IsAdmin)
                throw new FlightReservationException(400,
                    "viewing flights for other users is only available by admins.");
        }

        var reservations = await _reservationRepository.GetReservations(ct,
            request.flightId,
            request.customerId ?? currentUser.Id);

        var flights = await _flightRepository.GetFlights(ct,
            reservations.Select(x => x.FlightId).Distinct().ToArray());

        var users = await _userRepository.GetUsers(ct,
            reservations.Select(x => x.UserId).Distinct().ToArray());

        User user = new();
        Flight flight = new();

        foreach (var reservation in reservations)
        {
            user = users.FirstOrDefault(x => x.Id.Equals(reservation.UserId));
            if (user is not null)
                reservation.SetUser(user);

            flight = flights.FirstOrDefault(x => x.Id.Equals(reservation.FlightId));
            if (flight is not null)
                reservation.SetFlight(flight);
        }

        return new GetReservationsQueryResponse(reservations);
    }
}