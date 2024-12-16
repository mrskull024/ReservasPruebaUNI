using Microsoft.EntityFrameworkCore;
using ReservasPruebaUNI.Models;

namespace ReservasPruebaUNI.Services
{
    public class ReservasContext : DbContext
    {
        public ReservasContext(DbContextOptions<ReservasContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
