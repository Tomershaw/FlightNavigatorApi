using Shared;

namespace FlightNavigatorApi.Model
{
    public class FlightLogger
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public Leg FromLeg { get; set; }
        public Leg ToLeg { get; set; }
        public DateTime? DateFlight { get; set; }
    }
}
