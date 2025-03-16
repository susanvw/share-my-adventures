using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.HasOne(x => x.Participant).WithMany(x => x.Positions).HasForeignKey(x => x.ParticipantId).IsRequired();
        builder.Property(t => t.Latitude).IsRequired();
        builder.Property(t => t.Longitude).IsRequired();
    }
}
