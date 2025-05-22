using Microsoft.EntityFrameworkCore;

namespace c4_DataCaching.DataAccess;

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
        builder.Entity<Customer>()
            .HasKey(c => c.Id);
        builder.Entity<Customer>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        builder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();
        builder.Entity<Customer>()
            .Property(c => c.FirstName)
            .IsRequired();

        builder.Entity<Customer>().HasData(new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@cookbook.com"
        });
    }
}