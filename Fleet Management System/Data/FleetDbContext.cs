using Fleet_Management_System.Model;
using Microsoft.EntityFrameworkCore;

public class FleetDbContext : DbContext
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleLocation> VehicleLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Vehicles table
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.RegistrationNumber)
            .IsUnique();

        modelBuilder.Entity<VehicleLocation>()
            .Property(vl => vl.Timestamp)
            .IsRequired();

        modelBuilder.Entity<VehicleLocation>()
            .HasOne(vl => vl.Vehicle)
            .WithMany(v => v.VehicleLocations)
            .HasForeignKey(vl => vl.VehicleID);
    }
}