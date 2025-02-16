namespace FlightReservation.Domain.Primitives;


public record ResponseWrapper
{
    public ResponseWrapper(
        bool isSuccess = true,
        int statusCode = 200,
        string? message = null
        , object? result = null
    )
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message;
        Result = result;
    }

    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }

    public string? Message { get; set; }
    public object? Result { get; set; }
}