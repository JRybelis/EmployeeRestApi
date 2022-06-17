using EmployeeRestApi.Helpers;
using EmployeeRestApiLibrary.Models;
using Microsoft.EntityFrameworkCore;
namespace EmployeeRestApi.Data;

public class DataContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Address> Addresses { get; set; }


    public DataContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            // .UseLoggerFactory(loggerFactory)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureEntityPrimaryKeys(modelBuilder);
        ConfigureEntityProperties(modelBuilder);
        ConfigureEntityRelationships(modelBuilder);
        modelBuilder.ApplyUtcDateTimeConverter();
    }

    private static void ConfigureEntityPrimaryKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasKey(e => e.Id);
        modelBuilder.Entity<Address>().HasKey(a => a.AddressId);
    }

    private static void ConfigureEntityProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.FirstName)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.LastName)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.BirthDate)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmploymentCommencementDate)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.CurrentSalary)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Role)
            .IsRequired();

        modelBuilder.Entity<Address>()
            .Property(a => a.City)
            .IsRequired();

        modelBuilder.Entity<Address>()
            .Property(a => a.Street)
            .IsRequired();

        modelBuilder.Entity<Address>()
            .Property(a => a.PostCode)
            .IsRequired();
    }

    private static void ConfigureEntityRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.HomeAddress)
            .WithOne(a => a.Employee)
            .HasForeignKey<Address>(a => a.EmployeeId);
        modelBuilder.Entity<Employee>().ToTable("Employee");
        modelBuilder.Entity<Address>().ToTable("Address");
    }
}