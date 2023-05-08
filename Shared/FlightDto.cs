using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class FlightDto
    {
        public string FlightNumber { get; set; }

        public bool IsArrival { get; set; }

        public string? AirLine { get; set; }
        public Leg Leg { get; set; }

        public DateTime CreatedAt { get; set; }

        private static readonly string[] _airlines = { "Delta Air", "American Airlines", "Lufthansa", "Air France", "Southwest", "Emirates", "British Airways", "El Al", "EasyJet", "AirAsia" };
        private static readonly string[] _airlineCodes = { "DA", "AA", "L", "AF", "SW", "EM", "BA", "EA", "EJ", "AS" };

        public FlightDto()
        {
            Random random = new();
            int num = random.Next(1000000);
            random = new();
            IsArrival = random.Next(0, 2) == 0;

            CreatedAt = DateTime.Now;
            int index = random.Next(_airlineCodes.Length);
            AirLine = _airlines[index];
            FlightNumber = $"{_airlineCodes[index]}{num}";
            Leg = Leg.LegWaiting;
        }

        public override string ToString()
        {
            var status = IsArrival ? "Departing" : "Arriving";
            return $"flight number: {FlightNumber}, status:{status}, registered at airport at:{CreatedAt:G}";
        }
    }
}
