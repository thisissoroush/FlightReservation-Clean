using System.Text;
using FlightReservation.Application.Interfaces.Services;
using FlightReservation.Domain.Primitives;
using Microsoft.AspNetCore.Authentication;

namespace FlightReservation.Presentation.Services;

public class EndpointsFilter : IEndpointFilter
{
    public IJwtService _JwtService { get; set; }
    public EndpointsFilter(IJwtService jwtService)
    {
        _JwtService = jwtService;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {

        try
        {
            var rp = context.Arguments.Where(a => a is DefaultRequestParams).FirstOrDefault();

            if (rp != null && rp is DefaultRequestParams a)
            {
                a.IpAddress = 
                    context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? String.Empty;

                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var token = await context.HttpContext.GetTokenAsync("access_token");
                    a.UserId  = _JwtService.GetUserId(token.ToString());

                    if (a.UserId.Equals(default))
                        throw new Exception("Request User is invalid");
                }
            }

            var result = await next(context);

            // return result;
            return new ResponseWrapper(true, 200, null, result);
        }
        catch (FlightReservationException ex)
        {
            StringBuilder message = new(ex.Message);

            if (!string.IsNullOrEmpty(ex.TechnicalMessage))
                message.Append($"| Technical Message: {ex.TechnicalMessage}");

            return new ResponseWrapper(false, ex.StatusCode, message.ToString());
        }
        catch (Exception ex)
        {
            StringBuilder message = new("An unhandled error occured!");

            return new ResponseWrapper(false, 500, message.ToString());
        }
    }

}