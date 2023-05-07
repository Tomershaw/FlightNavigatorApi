using FlightNavigatorApi.DAL;
using FlightNavigatorApi.Model;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace FlightNavigatorApi.BusinessLogic
{
    public class FlightsLogic : IFlightsLogic
    {
        private readonly DbData _dataContext;
        private readonly ILogger<FlightsLogic> _logger;
        //private readonly Leg _dataLeg;

        public FlightsLogic(ILogger<FlightsLogic> logger, DbData dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public async Task MovePlanes()
        {

            var allActiveFlights = _dataContext.Flight
                                                .Where(x => x.Leg < Leg.LegFinshed)
                                                .ToLookup(x => x.Leg)
                                                .ToDictionary(x => x.Key, x => x.ToList());
            HandleLegNine(allActiveFlights);
            HandleLegEight(allActiveFlights);
            HandleLegSixSeven(allActiveFlights, Leg.Leg7);
            HandleLegSixSeven(allActiveFlights, Leg.Leg6);
            HandleLegFive(allActiveFlights);
            HandleLegFour(allActiveFlights);
            HandleLegThree(allActiveFlights);
            HandleLegOneTwo(allActiveFlights, Leg.Leg2, Leg.Leg3);
            HandleLegOneTwo(allActiveFlights, Leg.Leg1, Leg.Leg2);
            HandleLegQueue(allActiveFlights);
            HandleLegWaiting(allActiveFlights);
            await _dataContext.SaveChangesAsync();
        }

        private void HandleLegNine(Dictionary<Leg, List<Flight>> allActiveFlights)
        {
            if (!allActiveFlights.ContainsKey(Leg.Leg9) || !allActiveFlights[Leg.Leg9].Any())
            {
                return;
            }
            foreach (var flight in allActiveFlights[Leg.Leg9].ToList())
            {
                MoveFlight(allActiveFlights, flight, Leg.LegFinshed);
            }
        }

        private void HandleLegEight(Dictionary<Leg, List<Flight>> allActiveFlights)
        {
            if (!allActiveFlights.ContainsKey(Leg.Leg8) || !allActiveFlights[Leg.Leg8].Any())
            {
                return;
            }

            var flight = allActiveFlights[Leg.Leg8].First();

            if (!allActiveFlights.ContainsKey(Leg.Leg4) || !allActiveFlights[Leg.Leg4].Any())
            {
                MoveFlight(allActiveFlights, flight, Leg.Leg4);
            }
        }

        private void HandleLegSixSeven(Dictionary<Leg, List<Flight>> allActiveFlights, Leg legFrom)
        {
            if (!allActiveFlights.ContainsKey(legFrom) || !allActiveFlights[legFrom].Any())
            {
                return;
            }

            var flight = allActiveFlights[legFrom].First();


            if (flight.IsArrival == true)
            {
                MoveFlight(allActiveFlights, flight, Leg.LegFinshed);
            }
            else
            {
                if (!allActiveFlights.ContainsKey(Leg.Leg8) || !allActiveFlights[Leg.Leg8].Any())
                {
                    MoveFlight(allActiveFlights, flight, Leg.Leg8);
                }
            }
        }

        private void HandleLegFive(Dictionary<Leg, List<Flight>> allActiveFlights)
        {
            if (!allActiveFlights.ContainsKey(Leg.Leg5) || !allActiveFlights[Leg.Leg5].Any())
            {
                return;
            }

            var first2FlightsInLeg5 = allActiveFlights[Leg.Leg5].OrderBy(x => x.CreatedAt).Take(2);

            foreach (var flight in first2FlightsInLeg5.ToList())
            {
                if (!allActiveFlights.ContainsKey(Leg.Leg6) || !allActiveFlights[Leg.Leg6].Any())
                {
                    MoveFlight(allActiveFlights, flight, Leg.Leg6);
                }
                else if (!allActiveFlights.ContainsKey(Leg.Leg7) || !allActiveFlights[Leg.Leg7].Any())
                {
                    MoveFlight(allActiveFlights, flight, Leg.Leg7);
                }
            }
        }

        private void HandleLegFour(Dictionary<Leg, List<Flight>> allActiveFlights)
        {
            if (!allActiveFlights.ContainsKey(Leg.Leg4) || !allActiveFlights[Leg.Leg4].Any())
            {
                return;
            }

            var flightInLeg4 = allActiveFlights[Leg.Leg4].First();
            if (flightInLeg4.IsArrival == true)
            {
                MoveFlight(allActiveFlights, flightInLeg4, Leg.Leg5);
            }
            else
            {
                MoveFlight(allActiveFlights, flightInLeg4, Leg.Leg9);
            }
        }

        private void HandleLegOneTwo(Dictionary<Leg, List<Flight>> allActiveFlights, Leg fromLeg, Leg toLeg)
        {
            if (!allActiveFlights.ContainsKey(fromLeg) || !allActiveFlights[fromLeg].Any())
            {
                return;
            }

            foreach (var flight in allActiveFlights[fromLeg].ToList())
            {
                MoveFlight(allActiveFlights, flight, toLeg);
            }


        }
        private void HandleLegThree(Dictionary<Leg, List<Flight>> allActiveFlights)
        {
            if (!allActiveFlights.ContainsKey(Leg.Leg3) || !allActiveFlights[Leg.Leg3].Any())
            {
                return;
            }

            var oldestFlightInLeg3 = allActiveFlights[Leg.Leg3].OrderBy(x => x.CreatedAt).First();

            if (!allActiveFlights.ContainsKey(Leg.Leg4) || !allActiveFlights[Leg.Leg4].Any())
            {
                MoveFlight(allActiveFlights, oldestFlightInLeg3, Leg.Leg4);
            }
        }

        private void HandleLegQueue(Dictionary<Leg, List<Flight>> allActiveFlights)
        {
            if (!allActiveFlights.ContainsKey(Leg.LegQueue) || !allActiveFlights[Leg.LegQueue].Any())
            {
                return;
            }

            var oldestFlightInQueueLeg = allActiveFlights[Leg.LegQueue].OrderBy(x => x.CreatedAt).First();

            if (!allActiveFlights.ContainsKey(Leg.Leg6) || !allActiveFlights[Leg.Leg6].Any())
            {
                MoveFlight(allActiveFlights, oldestFlightInQueueLeg, Leg.Leg6);
            }
            else if (!allActiveFlights.ContainsKey(Leg.Leg7) || !allActiveFlights[Leg.Leg7].Any())
            {
                MoveFlight(allActiveFlights, oldestFlightInQueueLeg, Leg.Leg7);
            }
        }

        private void HandleLegWaiting(Dictionary<Leg, List<Flight>> allActiveFlights)
        {
            if (!allActiveFlights.ContainsKey(Leg.LegWaiting))
            {
                return;
            }
            foreach (var flight in allActiveFlights[Leg.LegWaiting].ToList())
            {
                if (flight.IsArrival == true)
                {
                    MoveFlight(allActiveFlights, flight, Leg.Leg1);
                }
                else
                {
                    var oldestFlightInQueueLeg = allActiveFlights.ContainsKey(Leg.LegQueue) ?
                                                                            allActiveFlights[Leg.LegQueue].OrderBy(x => x.CreatedAt).FirstOrDefault() :
                                                                            null;
                    if (oldestFlightInQueueLeg != null) // queue already populated 
                    {
                        MoveFlight(allActiveFlights, flight, Leg.LegQueue);
                    }
                    else // queue is empty 
                    {
                        if (!allActiveFlights.ContainsKey(Leg.Leg6) || !allActiveFlights[Leg.Leg6].Any())
                        {
                            MoveFlight(allActiveFlights, flight, Leg.Leg6);
                        }
                        else if (!allActiveFlights.ContainsKey(Leg.Leg7) || !allActiveFlights[Leg.Leg7].Any())
                        {
                            MoveFlight(allActiveFlights, flight, Leg.Leg7);
                        }
                        else
                        {
                            MoveFlight(allActiveFlights, flight, Leg.LegQueue);
                        }
                    }
                }

            }
        }

        private void MoveFlight(Dictionary<Leg, List<Flight>> allActiveFlights, Flight flight, Leg newLeg)
        {
            _logger.LogDebug($"Moving flight {flight.FlightNumber} from {flight.Leg} to {newLeg}");
            if (flight.FlightLoggers == null)
            {
                flight.FlightLoggers = new List<FlightLogger>();
            }
            flight.FlightLoggers.Add(new FlightLogger()
            {
                DateFlight = DateTime.Now,
                FromLeg = flight.Leg,
                ToLeg = newLeg,
            });

            allActiveFlights[flight.Leg].Remove(flight);
            if (!allActiveFlights.ContainsKey((Leg)newLeg))
            {
                allActiveFlights.Add(newLeg, new List<Flight>());
            }
            allActiveFlights[newLeg].Add(flight);

            flight.Leg = newLeg;
        }

    }
}
