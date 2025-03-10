using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class AdventureConfiguration : IEntityTypeConfiguration<Adventure>
{
    public void Configure(EntityTypeBuilder<Adventure> builder)
    {
        builder.Property(t => t.Name).IsRequired().HasMaxLength(32);
        builder.Property(t => t.StartDate).IsRequired();
        builder.Property(t => t.EndDate).IsRequired();

        builder.HasOne(x => x.TypeLookup).WithMany().HasForeignKey(x => x.TypeLookupId).IsRequired();
        builder.HasOne(x => x.StatusLookup).WithMany().HasForeignKey(x => x.StatusLookupId).IsRequired();


        builder.HasOne(x => x.MeetupLocationLookup).WithMany().HasForeignKey(x => x.MeetupLocationId);
        builder.HasOne(x => x.DestinationLocationLookup).WithMany().HasForeignKey(x => x.DestinationLocationId);
    }
}
 