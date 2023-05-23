using FlightNavigatorApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightNavigatorApi.DAL
{
    public class DbData : DbContext, IDbData
    {
        public DbData(DbContextOptions<DbData> options) : base(options)
        {

        }

        public DbSet<Flight> Flight { get; set; }
        public DbSet<FlightLogger> FlightLogger { get; set; }
    }
}
