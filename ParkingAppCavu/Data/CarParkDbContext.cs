using Microsoft.EntityFrameworkCore;
using ParkingAppCavu.Models;

namespace ParkingAppCavu.Data
{
    public class CarParkDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }

        public CarParkDbContext(DbContextOptions<CarParkDbContext> options)
            : base(options)
        {
        }
    }

}
