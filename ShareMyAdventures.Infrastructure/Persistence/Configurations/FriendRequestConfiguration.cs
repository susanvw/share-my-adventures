using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    { 
        // Configure the relationship to the participant who sends the invite
        builder.HasOne(f => f.Participant)
            .WithMany(p => p.Friends)
            .HasForeignKey(f => f.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship to the participant who receives the invite
        builder.HasOne(f => f.ParticipantFriend)
            .WithMany()
            .HasForeignKey(f => f.ParticipantFriendId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(e => e.InvitationStatusLookup, isl =>
        {
            isl.Property(i => i.Id)
               .HasColumnName("InvitationStatusLookupId")
               .IsRequired();
            isl.Property(i => i.Name)
               .HasColumnName("InvitationStatusLookupName")
               .IsRequired()
               .HasMaxLength(50);
        });

        builder.HasIndex(x => new { x.ParticipantFriendId, x.ParticipantId }).IsUnique();
    }
}