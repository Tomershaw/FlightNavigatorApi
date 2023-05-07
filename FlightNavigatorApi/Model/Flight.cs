using NLog.LayoutRenderers;

namespace FlightNavigatorApi.Model
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public bool IsArrival { get; set; }
        public Leg Leg { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<FlightLogger> FlightLoggers { get; set; }
    }
}

