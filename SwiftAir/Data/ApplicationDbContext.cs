using Microsoft.EntityFrameworkCore;
using SwiftAir.Models;

namespace SwiftAir.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public object Bookings { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=Tomas;" +
                "Database=SwiftAirDB;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;");
        }   
    }
}
