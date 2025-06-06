using Microsoft.EntityFrameworkCore;

namespace c4_LocalDatabaseConnection.DataAccess;

public class CrmContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "localdatabase.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Customer>().HasData(new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@cookbook.com"
        });
    }
}