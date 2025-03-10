namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Abstract base class for implementing value objects in the domain model.
/// Value objects are immutable objects that are defined by their attributes rather than an identity.
/// They are used to represent descriptive elements in the domain with no conceptual identity.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Determines whether two value objects are equal by comparing their equality components.
    /// Handles null checks to ensure proper equality logic.
    /// </summary>
    /// <param name="left">The first value object to compare.</param>
    /// <param name="right">The second value object to compare.</param>
    /// <returns>True if the value objects are equal or both null; false otherwise.</returns>
    private static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        // XOR (^) ensures that if one is null and the other isn't, they aren't equal
        if (left is null ^ right is null) return false;

        // If both are null or both are non-null, compare using Equals
        return left?.Equals(right!) != false;
    }

    /// <summary>
    /// Determines whether two value objects are not equal by negating the result of the equality check.
    /// </summary>
    /// <param name="left">The first value object to compare.</param>
    /// <param name="right">The second value object to compare.</param>
    /// <returns>True if the value objects are not equal; false otherwise.</returns>
    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
    {
        return !EqualOperator(left, right);
    }

    /// <summary>
    /// Defines the components of the value object that contribute to its equality.
    /// Derived classes must implement this to specify which properties or fields are used for equality checks.
    /// </summary>
    /// <returns>An enumerable of objects representing the equality components.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Compares this value object with another object for equality.
    /// Overrides the default Equals method to implement value-based equality.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>True if the objects are equal based on their equality components; false otherwise.</returns>
    public override bool Equals(object? obj)
    {
        // Early return if obj is null or types don't match
        if (obj == null || obj.GetType() != GetType()) return false;

        var other = (ValueObject)obj;
        // Compare sequences of equality components
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Generates a hash code for the value object based on its equality components.
    /// Overrides the default GetHashCode method to ensure consistent hashing for equal objects.
    /// </summary>
    /// <returns>A hash code computed from the equality components.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y); // XOR combines hash codes
    }
}