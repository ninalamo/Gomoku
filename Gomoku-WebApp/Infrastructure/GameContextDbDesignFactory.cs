using System.Reflection;
using Gomoku.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gomoku_WebApp.Infrastructure;

public class GameContextDbDesignFactory : IDesignTimeDbContextFactory<GameContext>
{
    public GameContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<GameContext>();
        optionsBuilder.UseSqlServer(
            connectionString, 
            x => x.MigrationsAssembly(Assembly.GetAssembly(typeof(GameContext))?.GetName().Name));

        return new GameContext(optionsBuilder.Options);
    }
}