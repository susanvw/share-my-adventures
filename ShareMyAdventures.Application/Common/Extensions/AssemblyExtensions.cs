// Application Project: Extensions/AssemblyExtensions.cs
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ShareMyAdventures.Application.Common.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to add services from an assembly.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Adds services from the specified assembly that implement the given interface type with a scoped lifetime.
    /// Supports both generic (e.g., <c>IHandler&lt;T&gt;</c>) and non-generic interfaces (e.g., <c>IHandler</c>).
    /// </summary>
    /// <typeparam name="TInterface">The interface type (generic or non-generic) that services should implement.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assembly">The assembly to scan for implementations of the interface.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="assembly"/> is null.</exception>
    /// <remarks>
    /// Only concrete (non-abstract, non-interface) types are registered. For generic interfaces, only types implementing
    /// constructed versions of <typeparamref name="TInterface"/> (e.g., <c>IHandler&lt;string&gt;</c>) are included.
    /// Each implementation is registered against its matching interface(s).
    /// </remarks>
    /// <example>
    /// <code>
    /// services.AddItemsFromAssembly&lt;IHandler&gt;(typeof(MyHandler).Assembly);
    /// </code>
    /// </example>
    public static void AddItemsFromAssembly<TInterface>(this IServiceCollection services, Assembly assembly)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (assembly == null) 
            throw new ArgumentNullException(nameof(assembly));

        // Determine if TInterface is generic
        var interfaceType = typeof(TInterface);
        var isGenericInterface = interfaceType.IsGenericTypeDefinition;

        // Find all concrete types in the assembly that implement TInterface
        var interfaceImplementations = assembly.GetTypes()
            .Where(type =>
                !type.IsAbstract && // Exclude abstract classes
                !type.IsInterface && // Exclude interfaces
                type.GetInterfaces().Any(i =>
                    isGenericInterface
                        ? i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType
                        : i == interfaceType))
            .ToList();

        foreach (var implementationType in interfaceImplementations)
        {
            // Get only the interfaces that match TInterface (generic or non-generic)
            var matchingInterfaces = implementationType.GetInterfaces()
                .Where(i =>
                    isGenericInterface
                        ? i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType
                        : i == interfaceType);

            foreach (var matchingInterface in matchingInterfaces)
            {
                services.AddScoped(matchingInterface, implementationType);
            }
        }
    }
}