using EmployeeRestApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace EmployeeRestApiUnitTests.Helpers;

public class InMemoryDbContextRunner
{
    protected DataContext context;

    public InMemoryDbContextRunner(DataContext context = null)
    {
        this.context = context ?? GetInMemoryDbContext();
    }

    protected DataContext GetInMemoryDbContext()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var builder = new DbContextOptionsBuilder<DataContext>();
        var options = builder.UseInMemoryDatabase("test")
            .UseInternalServiceProvider(serviceProvider).Options;

        var dbContext = new DataContext(options);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}