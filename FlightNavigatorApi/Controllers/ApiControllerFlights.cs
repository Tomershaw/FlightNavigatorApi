using FlightNavigatorApi.DAL;
using FlightNavigatorApi.Hubs;
using FlightNavigatorApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<TerminalHub> _hub;
        public ApiControllerFlights(DbData context, ILogger<ApiControllerFlights> logger, IHubContext<TerminalHub> hub)
        {
            _dataContext = context;
            _logger = logger;
            _hub = hub;
        }
        [HttpGet]
        [Route("board")]
        public IActionResult Get()
        {
            _hub.Clients.All.SendAsync("TransferFlightsData", GetAllFlights());

            return Ok(new { Message = "Request Completed" });
        }
        private List<FlightDto> GetAllFlights()
        {
            return _dataContext.Flight
                                .Where(x => x.Leg < Leg.LegFinshed)
                                .Select(x => new FlightDto()
                                {
                                    AirLine = x.Airline,
                                    IsArrival = x.IsArrival,
                                    CreatedAt = x.CreatedAt,
                                    Leg = x.Leg,
                                    FlightNumber = x.FlightNumber
                                }).ToList();
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<FlightDto> GetAll()
        {
            return _dataContext.Flight
                                .Where(x => x.Leg < Leg.LegFinshed)
                                .Select(x => new FlightDto()
                                {
                                    AirLine = x.Airline,
                                    IsArrival = x.IsArrival,
                                    CreatedAt = x.CreatedAt,
                                    Leg = x.Leg,
                                    FlightNumber = x.FlightNumber
                                }).AsEnumerable();
        }

        [HttpPost]
        public async Task Post([FromBody] FlightDto value)
        {
            _dataContext.Add(new Flight
            {
                FlightNumber = value.FlightNumber,
                IsArrival = value.IsArrival,
                Leg = Leg.LegWaiting,
                Airline= value.AirLine,
                CreatedAt = value.CreatedAt
            });
            await _dataContext.SaveChangesAsync();

        }

    }
}
