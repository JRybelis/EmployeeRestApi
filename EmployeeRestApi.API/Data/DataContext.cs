using EmployeeRestApi.Helpers;
using EmployeeRestApiLibrary.Models;
using Microsoft.EntityFrameworkCore;
namespace EmployeeRestApi.Data;

public class DataContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }


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
        modelBuilder.ApplyUtcDateTimeConverter();
    }

    private static void ConfigureEntityPrimaryKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasKey(e => e.Id);
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
            .Property(e => e.StartDate)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.CurrentSalary)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Role)
            .IsRequired();
    }
}