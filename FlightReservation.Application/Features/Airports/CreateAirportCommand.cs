using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Domain.Entities;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Airports;

public record CreateAirportCommand(string Code, string Name, string City, string Country) : DefaultRequestParams,IRequest;

public class CreateAirportCommandHandler : IRequestHandler<CreateAirportCommand>
{
    private readonly IAirportRepository _airportRepository;
    private readonly IMediator _mediator;
    public CreateAirportCommandHandler(IAirportRepository airportRepository, IMediator mediator)
    {
        _airportRepository = airportRepository;
        _mediator = mediator;
    }
    public async Task Handle(CreateAirportCommand request, CancellationToken ct)
    {
        var existingAirpot = await _airportRepository.GetAirport(ct, request.Code);

        if (existingAirpot is not null)
            throw new FlightReservationException(400,"Flight with this number already exists");
       
        var flight = new Airport(request.Code,
            request.Name,
            request.City,
            request.Country);

        try
        {
            await _airportRepository.AddAirport(ct, flight);
        }
        catch (Exception ex)
        {
            throw new FlightReservationException(500, "An unhandled error occured.",ex.Message);
        }
    }
}
