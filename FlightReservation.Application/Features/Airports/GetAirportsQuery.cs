using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Airports;

public record GetAirportsQuery(
    string? Code,
    string? Name,
    string City,
    string Country
) : IRequest<GetAirportsQueryResponse>;


public record GetAirportsQueryResponse(List<Airport> Airports);

public class GetAirportsQueryHandler : IRequestHandler<GetAirportsQuery, GetAirportsQueryResponse>
{
    private readonly IAirportRepository _airportRepository;
    public GetAirportsQueryHandler(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }
    public async Task<GetAirportsQueryResponse> Handle(GetAirportsQuery request, CancellationToken ct)
    {
        var airports =  await _airportRepository.GetAirports(ct, 
            request.Code, 
            request.Name,
            request.City,
            request.Country);

        if (airports is null || !airports.Any())
            throw new FlightReservationException(400, "No airports found");
        
        return new GetAirportsQueryResponse(airports);
    }
}