using FlightNavigatorApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightNavigatorApi.DAL
{
    public interface IDbData
    {
        DbSet<Flight> Flight { get; set; }
        DbSet<FlightLogger> FlightLogger { get; set; }
    }
}