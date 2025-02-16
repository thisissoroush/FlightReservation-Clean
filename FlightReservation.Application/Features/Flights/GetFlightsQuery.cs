using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using MediatR;

namespace FlightReservation.Application.Features.Flights;

public record GetFlightsQuery(
    int SourceAirportId,
    int destinationAirportId,
    DateTime? startDate,
    DateTime? endDate,
    bool? OnlyAvailable
) : IRequest<GetFlightsQueryResponse>;

public record GetFlightsQueryResponse(List<Flight> Flights);

public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, GetFlightsQueryResponse>
{
    private readonly IFlightRepository _flightRepository;
    public GetFlightsQueryHandler(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }
    public async Task<GetFlightsQueryResponse> Handle(GetFlightsQuery request, CancellationToken ct)
    {
        var flights = await _flightRepository.GetFlights(ct, 
            request.SourceAirportId,
            request.destinationAirportId,
            request.startDate,
            request.endDate,
            request.OnlyAvailable);

        return new GetFlightsQueryResponse(flights);
    }
}