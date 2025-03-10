using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class FriendConfiguration : IEntityTypeConfiguration<FriendRequest>
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



        builder.HasOne(x => x.InvitationStatusLookup).WithMany().HasForeignKey(x => x.InvitationStatusLookupId).IsRequired();


        builder.HasIndex(x => new { x.ParticipantFriendId, x.ParticipantId }).IsUnique();
    }
}