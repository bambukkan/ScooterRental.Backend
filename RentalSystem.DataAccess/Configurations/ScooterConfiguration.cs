

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Core.Models;

public class ScooterConfiguration() : IEntityTypeConfiguration<ScooterEntity>
{
    public void Configure(EntityTypeBuilder<ScooterEntity> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.SerialNumber).IsUnique();

        builder
            .HasMany(s => s.Bookings)
            .WithOne(b => b.Scooter).
            HasForeignKey(b => b.ScooterId);
    }
}