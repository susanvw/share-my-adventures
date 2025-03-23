using MediatR;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.InvitationAggregate;
using ShareMyAdventures.Domain.Entities.LocationAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace ShareMyAdventures.Infrastructure.Persistence;

public sealed class ApplicationDbContext(
	DbContextOptions<ApplicationDbContext> options,
	AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
	IMediator mediator

	) : IdentityDbContext<Participant>(options)
{
     
    public DbSet<Adventure> Adventures => Set<Adventure>();
    public DbSet<ParticipantAdventure> ParticipantAdventures => Set<ParticipantAdventure>();
    public DbSet<AdventureInvitation> AdventureInvitations => Set<AdventureInvitation>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<FriendList> FriendLists => Set<FriendList>();
    public DbSet<FriendRequest> Friends => Set<FriendRequest>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Position> Positions => Set<Position>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<BaseEntity> source = from e in this.ChangeTracker.Entries<BaseEntity>()
                                         where e.Entity.DomainEvents.Count != 0
                                         select e.Entity;
        var baseEntities = source as BaseEntity[] ?? source.ToArray();
        var list = baseEntities.SelectMany( e => e.DomainEvents).ToList();
        baseEntities.ToList().ForEach(delegate (BaseEntity e)
        {
            e.ClearDomainEvents();
        });
        foreach (var item in list)
        {
            await mediator.Publish(item, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
