using ShareMyAdventures.Domain.Entities.LocationAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class LocationLookupConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Property(x => x.AddressLine1).HasMaxLength(256).IsRequired();
        builder.Property(x => x.AddressLine2).HasMaxLength(256).IsRequired();


        builder.Property(x => x.Suburb).HasMaxLength(64);
        builder.Property(x => x.City).HasMaxLength(64);
        builder.Property(x => x.Province).HasMaxLength(64);
        builder.Property(x => x.PostalCode).HasMaxLength(16);


        builder.Property(x => x.Country).HasMaxLength(64).IsRequired();
    }
}
