using System.Reflection;
using FluentValidation;
using Gomoku.Application.Behaviors;
using Gomoku.Application.Commands.AddGame;
using Gomoku.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gomoku_WebApp;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<GameContext>(options => 
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(GameContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                });
                } //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );

        return services;
    }
    
    public static IServiceCollection AddMediatorBundles(this IServiceCollection services)
    {

        //register mediatr and pipelines
        services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(AddGameCommand).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

        // Register command and query handlers
        services.AddScoped<IRequestHandler<AddGameCommand, AddGameCommandResult>, AddGameCommandHandler>();

        // Register validators
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(AddGameCommandValidator)));
        return services;
    }
    
}