

using Microsoft.EntityFrameworkCore;
using RentalSystem.Core.Models;

public class RentalSystemDbContext(DbContextOptions<RentalSystemDbContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings {get;set;}
    public DbSet<ScooterEntity> Scooters {get;set;}
    public DbSet<UserEntity> Users {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new ScooterConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}