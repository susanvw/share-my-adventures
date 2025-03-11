using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class AdventureInvitationConfiguration : IEntityTypeConfiguration<AdventureInvitation>
{
    public void Configure(EntityTypeBuilder<AdventureInvitation> builder)
    {
        // Primary key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Email)
               .IsRequired()
               .HasMaxLength(256);

        // Relationships
        builder.HasOne(e => e.Adventure)
               .WithMany(a => a.Invitations) // Assuming Adventure has Invitations collection
               .HasForeignKey(e => e.AdventureId)
               .IsRequired();

        builder.OwnsOne(e => e.AccessLevelLookup, al =>
        {
            al.Property(a => a.Id).HasColumnName("AccessLevelLookupId").IsRequired();
            al.Property(a => a.Name).HasColumnName("AccessLevelLookupName").IsRequired().HasMaxLength(50);
        });

        builder.OwnsOne(e => e.InvitationStatusLookup, isl =>
        {
            isl.Property(i => i.Id).HasColumnName("InvitationStatusLookupId").IsRequired();
            isl.Property(i => i.Name).HasColumnName("InvitationStatusLookupName").IsRequired().HasMaxLength(50);
        });
    }
}