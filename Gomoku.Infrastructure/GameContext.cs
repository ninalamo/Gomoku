using Gomoku.Domain.AggregatesModel;
using Gomoku.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.Infrastructure;

public class GameContext : DbContext, IUnitOfWork
{
   public GameContext(DbContextOptions<GameContext>options):base(options)
   {
   }
   
   public DbSet<Game> Games { get; set; }

   public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
   {
      return base.SaveChangesAsync(cancellationToken);
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameContext).Assembly);
   }
}