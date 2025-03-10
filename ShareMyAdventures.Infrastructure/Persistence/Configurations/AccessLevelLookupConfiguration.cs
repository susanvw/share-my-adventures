namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;

internal class AccessLevelLookupConfiguration : IEntityTypeConfiguration<AccessLevelLookup>
{
    public void Configure(EntityTypeBuilder<AccessLevelLookup> builder)
    {
        builder.Property(t => t.Name).IsRequired().HasMaxLength(16);
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
    }
}
