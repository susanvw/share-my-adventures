using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class AdventureConfiguration : IEntityTypeConfiguration<Adventure>
{
    public void Configure(EntityTypeBuilder<Adventure> builder)
    {
        builder.Property(t => t.Name).IsRequired().HasMaxLength(32);
        builder.Property(t => t.StartDate).IsRequired();
        builder.Property(t => t.EndDate).IsRequired();

        builder.OwnsOne(e => e.TypeLookup, tl =>
        {
            tl.Property(t => t.Id)
              .HasColumnName("TypeLookupId")
              .IsRequired();
            tl.Property(t => t.Name)
              .HasColumnName("TypeLookupName")
              .IsRequired()
              .HasMaxLength(50);
        });

        builder.OwnsOne(e => e.StatusLookup, sl =>
        {
            sl.Property(s => s.Id)
              .HasColumnName("StatusLookupId")
              .IsRequired();
            sl.Property(s => s.Name)
              .HasColumnName("StatusLookupName")
              .IsRequired()
              .HasMaxLength(50);
        });

        builder.HasOne(x => x.MeetupLocationLookup).WithMany().HasForeignKey(x => x.MeetupLocationId);
        builder.HasOne(x => x.DestinationLocationLookup).WithMany().HasForeignKey(x => x.DestinationLocationId);
    }
}
 