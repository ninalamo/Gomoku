using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gomoku.Infrastructure;

public class GameContextDesignFactory : IDesignTimeDbContextFactory<GameContext>
{
    public GameContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GameContext>()
            .UseSqlServer("Server=localhost;Database=gamedb;User Id=sa;Password=someThingComplicated1234;TrustServerCertificate=True;",
                x => x.MigrationsAssembly(Assembly.GetAssembly(typeof(GameContext))?.GetName().Name));

        return new GameContext(optionsBuilder.Options);
    }
}