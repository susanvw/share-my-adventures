using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class StatusLookupConfiguration : IEntityTypeConfiguration<StatusLookup>
{
    public void Configure(EntityTypeBuilder<StatusLookup> builder)
    {
        builder.Property(t => t.Name).IsRequired().HasMaxLength(16);
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
    }
}
