using FlightNavigatorApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightNavigatorApi.DAL
{
    public class DbData : DbContext
    {
        public DbData(DbContextOptions<DbData> options) : base(options)
        {
            
        }

        public DbSet<Flight> Flight { get; set; }
        public DbSet<FlightLogger> FlightLogger { get; set; }
    }
}
