using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.Property(t => t.DisplayName).HasMaxLength(32);

        // Configure one-to-many relationship for Friends
        builder.HasMany(p => p.Friends)
            .WithOne(f => f.Participant)
            .HasForeignKey(f => f.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

    }
}