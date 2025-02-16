namespace FlightReservation.Domain.Primitives;


public class FlightReservationException : Exception
{
    public FlightReservationException(int statusCode, string? message = null, string? technicalMessage = null)
        : base(message)
    {
        StatusCode = statusCode;
        Message = message;
        TechnicalMessage = technicalMessage;
    }

    public int StatusCode { get; private set; }
    public string? Message { get; private set; }
    public string? TechnicalMessage { get; private set; }

    public override string ToString()
    {
        string baseMessage = base.ToString();
        if (!string.IsNullOrEmpty(Message))
        {
            baseMessage = baseMessage.Replace(
                Message,
                $"{Message}, TechnicalMessage: {TechnicalMessage}"
            );
        }
        else
        {
            if (InnerException != null)
            {
                int index = baseMessage.IndexOf("--->");
                if (index >= 0)
                    baseMessage = baseMessage.Insert(
                        index,
                        $"TechnicalMessage: {TechnicalMessage}"
                    );
            }
            else
            {
                baseMessage = baseMessage + $" TechnicalMessage: {TechnicalMessage}";
            }
        }

        return baseMessage;
    }
}