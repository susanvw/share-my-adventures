using System.Reflection;

namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Abstract base class for creating type-safe BaseEnums in the domain model.
/// BaseEnums are value objects that represent a fixed set of constants with an ID and name,
/// providing better type safety and behavior than traditional enums.
/// </summary>
public abstract class BaseEnum : IComparable
{
    /// <summary>
    /// Gets the display name of the BaseEnum.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the BaseEnum.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEnum"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the BaseEnum.</param>
    /// <param name="name">The display name for the BaseEnum.</param>
    protected BaseEnum(int id, string name) => (Id, Name) = (id, name);

    /// <summary>
    /// Returns the display name of the BaseEnum as its string representation.
    /// </summary>
    /// <returns>The value of the <see cref="Name"/> property.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Retrieves all instances of the specified BaseEnum type defined as public static fields.
    /// </summary>
    /// <typeparam name="T">The type of the BaseEnum to retrieve.</typeparam>
    /// <returns>An enumerable collection of all instances of type <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> GetAll<T>() where T : BaseEnum =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                 .Select(f => f.GetValue(null))
                 .Cast<T>();

    /// <summary>
    /// Determines whether the current BaseEnum is equal to another object.
    /// Equality is based on matching types and IDs.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>True if the objects are equal; false otherwise.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEnum otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Generates a hash code for the BaseEnum based on its type and ID.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    /// <summary>
    /// Compares this BaseEnum to another based on their IDs.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>A value indicating the relative order of the objects being compared.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="obj"/> is not an <see cref="BaseEnum"/>.</exception>
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1; // Non-null is greater than null
        if (obj is not BaseEnum other)
            throw new ArgumentException($"Object must be of type {nameof(BaseEnum)}", nameof(obj));
        return Id.CompareTo(other.Id);
    }

    /// <summary>
    /// Determines whether two BaseEnums are equal.
    /// </summary>
    /// <param name="left">The first BaseEnum to compare.</param>
    /// <param name="right">The second BaseEnum to compare.</param>
    /// <returns>True if the BaseEnums are equal; false otherwise.</returns>
    public static bool operator ==(BaseEnum? left, BaseEnum? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two BaseEnums are not equal.
    /// </summary>
    /// <param name="left">The first BaseEnum to compare.</param>
    /// <param name="right">The second BaseEnum to compare.</param>
    /// <returns>True if the BaseEnums are not equal; false otherwise.</returns>
    public static bool operator !=(BaseEnum? left, BaseEnum? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether the first BaseEnum is less than the second based on their IDs.
    /// </summary>
    /// <param name="left">The first BaseEnum to compare.</param>
    /// <param name="right">The second BaseEnum to compare.</param>
    /// <returns>True if <paramref name="left"/> is less than <paramref name="right"/>; false otherwise.</returns>
    public static bool operator <(BaseEnum? left, BaseEnum? right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Determines whether the first BaseEnum is less than or equal to the second based on their IDs.
    /// </summary>
    /// <param name="left">The first BaseEnum to compare.</param>
    /// <param name="right">The second BaseEnum to compare.</param>
    /// <returns>True if <paramref name="left"/> is less than or equal to <paramref name="right"/>; false otherwise.</returns>
    public static bool operator <=(BaseEnum? left, BaseEnum? right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Determines whether the first BaseEnum is greater than the second based on their IDs.
    /// </summary>
    /// <param name="left">The first BaseEnum to compare.</param>
    /// <param name="right">The second BaseEnum to compare.</param>
    /// <returns>True if <paramref name="left"/> is greater than <paramref name="right"/>; false otherwise.</returns>
    public static bool operator >(BaseEnum? left, BaseEnum? right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Determines whether the first BaseEnum is greater than or equal to the second based on their IDs.
    /// </summary>
    /// <param name="left">The first BaseEnum to compare.</param>
    /// <param name="right">The second BaseEnum to compare.</param>
    /// <returns>True if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; false otherwise.</returns>
    public static bool operator >=(BaseEnum? left, BaseEnum? right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}