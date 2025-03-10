using System.Collections;

namespace ShareMyAdventures.Application.Common.Mappings;

/// <summary>
/// 
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static TDestination AutoMapForUpdate<TDestination>(this object source, TDestination destination)
       where TDestination : class, new()
    {
        source.MatchAndMapForUpdate(destination);

        return destination;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TDestination AutoMap<TDestination>(this object source)
        where TDestination : class, new()
    {
        var destination = Activator.CreateInstance<TDestination>();
        source.MatchAndMap(destination);

        return destination;
    }

    private static void MatchAndMapForUpdate<TSource, TDestination>(this TSource source, TDestination destination)
        where TSource : class, new()
        where TDestination : class, new()
    {
        var sourceProperties = source.GetType().GetProperties().ToList();
        var destinationProperties = destination.GetType().GetProperties().ToList();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty =
                destinationProperties.Find(item =>
                    string.Equals(item.Name, sourceProperty.Name, StringComparison.CurrentCultureIgnoreCase));

            if (destinationProperty == null)
                continue;

            var value = sourceProperty.GetValue(source, null);

            if (value != null)
            {
                destinationProperty.SetValue(destination, value, null);
            }
        }
    }

    private static void MatchAndMap<TSource, TDestination>(this TSource? source, TDestination? destination)
      where TSource : class, new()
      where TDestination : class, new()
    {
        if (source == null || destination == null)
            return;

        var sourceProperties = source.GetType().GetProperties().ToList();
        var destinationProperties = destination.GetType().GetProperties().ToList();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty =
                destinationProperties.Find(item =>
                    string.Equals(item.Name, sourceProperty.Name, StringComparison.CurrentCultureIgnoreCase));

            if (destinationProperty == null || !destinationProperty.CanWrite)
                continue;

            var sourceValue = sourceProperty.GetValue(source);

            // Handle dictionary types
            if (sourceValue is IDictionary sourceDictionary &&
                destinationProperty.PropertyType.GetInterface(nameof(IDictionary)) != null)
            {
                var destinationDictionary = (IDictionary?)Activator.CreateInstance(destinationProperty.PropertyType);
                if (destinationDictionary != null)
                {
                    var valueType = destinationProperty.PropertyType.GetGenericArguments()[1];

                    foreach (DictionaryEntry entry in sourceDictionary)
                    {
                        var key = entry.Key;
                        var value = entry.Value;

                        var mappedKey = key;
                        var mappedValue = Activator.CreateInstance(valueType);

                        if (mappedValue != null && value != null)
                        {
                            value.MatchAndMap(mappedValue); // Recursively map values
                            destinationDictionary.Add(mappedKey, mappedValue);
                        }
                    }

                    destinationProperty.SetValue(destination, destinationDictionary);
                }
            }
            // Handle collections (e.g., List<T>, arrays)
            else if (sourceValue is IEnumerable sourceCollection && destinationProperty.PropertyType.IsArray)
            {
                var elementType = destinationProperty.PropertyType.GetElementType();
                if (elementType == null)
                    continue;

                var sourceArray = sourceCollection.Cast<object>().ToArray();

                // Check if types are directly compatible
                if (elementType.IsAssignableFrom(sourceArray.GetType().GetElementType()))
                {
                    // Create an array of the required type
                    var array = Array.CreateInstance(elementType, sourceArray.Length);
                    Array.Copy(sourceArray, array, sourceArray.Length);
                    destinationProperty.SetValue(destination, array);
                }
                else
                {
                    // Handle conversion of array elements
                    var array = Array.CreateInstance(elementType, sourceArray.Length);
                    for (int i = 0; i < sourceArray.Length; i++)
                    {
                        var element = Activator.CreateInstance(elementType);
                        sourceArray[i].MatchAndMap(element);
                        array.SetValue(element, i);
                    }

                    destinationProperty.SetValue(destination, array);
                }
            }
            // Handle complex types (not primitives)
            else if (sourceProperty.PropertyType.IsClass && !sourceProperty.PropertyType.Namespace?.StartsWith("System") == true)
            {
                var nestedDestination = Activator.CreateInstance(destinationProperty.PropertyType);
                sourceValue?.MatchAndMap(nestedDestination);
                destinationProperty.SetValue(destination, nestedDestination);
            }
            else
            {
                // Handle simple properties (primitives, strings, etc.)
                destinationProperty.SetValue(destination, sourceValue);
            }
        }
    }
}
