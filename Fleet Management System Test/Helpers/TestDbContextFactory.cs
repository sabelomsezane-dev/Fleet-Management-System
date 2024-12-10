namespace Fleet_Management_System_Test.Helpers
{
    using Microsoft.EntityFrameworkCore;

    public static class TestDbContextFactory
    {
        public static FleetDbContext CreateTestDbContext()
        {
            // Read the connection string from a configuration file (you can also hardcode it)
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Fleet_Management_DB;Trusted_Connection=True;MultipleActiveResultSets=true";

            var options = new DbContextOptionsBuilder<FleetDbContext>()
                .UseSqlServer(connectionString)  // Use the local database connection string
                .Options;

            var dbContext = new FleetDbContext(options);
            return dbContext;
        }
    }
}