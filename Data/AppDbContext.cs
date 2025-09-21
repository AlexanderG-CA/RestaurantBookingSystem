namespace RestaurantBookingSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using RestaurantBookingSystem.Models;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add indexes for performance
            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.BookingDate, b.StartTime });

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber);

            modelBuilder.Entity<Table>()
                .HasIndex(t => t.Capacity);
        }
    }
}
