using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class FriendListConfiguration : IEntityTypeConfiguration<FriendList>
{
    public void Configure(EntityTypeBuilder<FriendList> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(64).IsRequired();
        builder.HasMany(x => x.Friends).WithMany(x => x.FriendLists);

    }
}
