using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Airports;

public record GetAirportQuery(int Id) : IRequest<GetAirportQueryResponse>;


public record GetAirportQueryResponse(Airport Airport);

public class GetAirportQueryHandler : IRequestHandler<GetAirportQuery, GetAirportQueryResponse>
{
    private readonly IAirportRepository _airportRepository;

    public GetAirportQueryHandler(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }

    public async Task<GetAirportQueryResponse> Handle(GetAirportQuery request, CancellationToken ct)
    {
        var airport =  await _airportRepository.GetAirport(ct, request.Id);

        if (airport is null)
            throw new FlightReservationException(400, "No airport found");
        
        return new GetAirportQueryResponse(airport);
    }
}