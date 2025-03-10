namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Abstract base class for auditable entities in the ShareMyAdventures domain.
/// Extends <see cref="BaseEntity"/> to include audit properties for tracking creation and modification history.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the date and time when this entity was created.
    /// Defaults to the current UTC time at instantiation.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the identifier or name of the user or system that created this entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this entity was last modified, if applicable.
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// Gets or sets the identifier or name of the user or system that last modified this entity, if applicable.
    /// </summary>
    public string? LastModifiedBy { get; set; }
}