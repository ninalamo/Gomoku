using Gomoku.Domain.AggregatesModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gomoku.Infrastructure.EntityConfigurations;

public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("game", "dbo");
        builder.HasKey(a => a.Id);
       
        builder.Metadata
            .FindNavigation(nameof(Game.Cells))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasOne(b => b.PlayerTwo)
            .WithMany()
            .HasForeignKey("PlayerOneId")
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(b => b.PlayerTwo)
            .WithMany()
            .HasForeignKey("PlayerTwoId")
            .OnDelete(DeleteBehavior.NoAction);

    }
}

public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("player", "dbo");
        builder.HasKey(a => a.Id);
        
     
    }
}

public class CellEntityTypeConfiguration : IEntityTypeConfiguration<Cell>
{
    public void Configure(EntityTypeBuilder<Cell> builder)
    {
        builder.ToTable("cell", "dbo");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Color).HasDefaultValue(Pebbles.Empty);

        builder.Property<Guid>("GameId").IsRequired();

    }
}

