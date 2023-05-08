using FlightNavigatorApi.BusinessLogic;
using FlightNavigatorApi.DAL;
using FlightNavigatorApi.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightNavigatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerFlights : ControllerBase
    {
        private readonly DbData _dataContext;
        private readonly ILogger<ApiControllerFlights> _logger;
        private readonly IFlightsLogic _flightsLogic;

        public ApiControllerFlights(DbData context, ILogger<ApiControllerFlights> logger, IFlightsLogic flightsLogic)
        {
            _dataContext = context;
            _logger = logger;
            _flightsLogic = flightsLogic;
        }
        // GET: api/<ApiControllerFlights>
        [HttpGet]
        public IEnumerable<FlightDto> Get()
        {
            var allflightactive = _dataContext.Flight.Where(x => x.Leg < Leg.LegFinshed);
            return allflightactive.Select(x => new FlightDto()
            {
                AirLine =x.Airline,
                IsArrival= x.IsArrival,
                CreatedAt= x.CreatedAt,
                Leg = x.Leg,
                FlightNumber = x.FlightNumber
            }).AsEnumerable();

        }

        // GET api/<ApiControllerFlights>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApiControllerFlights>
        [HttpPost]
        public async Task Post([FromBody] FlightDto value)
        {
            _dataContext.Add(new Flight
            {
                FlightNumber = value.FlightNumber,
                IsArrival = value.IsArrival,
                Leg = Leg.LegWaiting,
                CreatedAt = value.CreatedAt
            });
            await _dataContext.SaveChangesAsync();
            await _flightsLogic.MovePlanes();


        }

        // PUT api/<ApiControllerFlights>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiControllerFlights>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
