using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class AdventureInvitationConfiguration : IEntityTypeConfiguration<AdventureInvitation>
{
    public void Configure(EntityTypeBuilder<AdventureInvitation> builder)
    {
        builder.Property(t => t.Email).IsRequired().HasMaxLength(256);
        builder.HasOne(x => x.Adventure).WithMany().HasForeignKey(x => x.AdventureId).IsRequired();
        builder.HasOne(x => x.AccessLevelLookup).WithMany().HasForeignKey(x => x.AccessLevelLookupId).IsRequired();
        builder.HasOne(x => x.InvitationStatusLookup).WithMany().HasForeignKey(x => x.InvitationStatusLookupId).IsRequired();
    }
}