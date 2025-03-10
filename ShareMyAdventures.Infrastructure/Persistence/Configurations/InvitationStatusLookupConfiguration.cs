namespace ShareMyAdventures.Infrastructure.Persistence.Configurations;
internal class InvitationStatusLookupConfiguration : IEntityTypeConfiguration<InvitationStatusLookup>
{
    public void Configure(EntityTypeBuilder<InvitationStatusLookup> builder)
    {
        builder.Property(t => t.Name).IsRequired().HasMaxLength(16);
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
    }
}
