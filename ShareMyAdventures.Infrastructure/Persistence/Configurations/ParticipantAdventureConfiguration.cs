using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class ParticipantAdventureConfiguration : IEntityTypeConfiguration<ParticipantAdventure>
{
    public void Configure(EntityTypeBuilder<ParticipantAdventure> builder)
    {

        builder.HasOne(x => x.Adventure).WithMany(x => x.Participants).HasForeignKey(x => x.AdventureId).IsRequired();
        builder.HasOne(x => x.Participant).WithMany(x => x.Adventures).HasForeignKey(x => x.ParticipantId).IsRequired();
        builder.HasOne(x => x.AccessLevelLookup).WithMany().HasForeignKey(x => x.AccessLevelLookupId).IsRequired();
         

        builder.Property(t => t.Distance).HasDefaultValue(0);
    }
}
