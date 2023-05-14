using NLog.LayoutRenderers;
using Shared;

namespace FlightNavigatorApi.Model
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Airline { get; set; } = string.Empty;
        public bool IsArrival { get; set; }
        public Leg Leg { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<FlightLogger> FlightLoggers { get; set; }
    }
}

