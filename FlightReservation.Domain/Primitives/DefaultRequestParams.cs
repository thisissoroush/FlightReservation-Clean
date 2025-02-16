using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FlightReservation.Domain.Primitives;


public record DefaultRequestParams
{
    [IgnoreDataMember]
    [JsonIgnore]
    public int UserId { get; set; }
    
    [IgnoreDataMember]
    [JsonIgnore]
    public string IpAddress { get; set; }
}