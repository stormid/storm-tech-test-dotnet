using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Storm.TechTask.SharedKernel.Authorization
{
    // Copied from Fluent Validation.
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all Authorizers in specified assemblies
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <param name="assemblies">The assemblies to scan</param>
        /// <param name="lifetime">The lifetime of the Authorizers. The default is scoped (per-request in web applications)</param>
        /// <param name="filter">Optional filter that allows certain types to be skipped from registration.</param>
        /// <param name="includeInternalTypes">Include internal Authorizers. The default is false.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizersFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            foreach (var assembly in assemblies)
            {
                services.AddAuthorizersFromAssembly(assembly, lifetime);
            }

            return services;
        }

        /// <summary>
        /// Adds all Authorizers in specified assembly
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <param name="lifetime">The lifetime of the Authorizers. The default is scoped (per-request in web application)</param>
        /// <param name="filter">Optional filter that allows certain types to be skipped from registration.</param>
        /// <param name="includeInternalTypes">Include internal Authorizers. The default is false.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizersFromAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AssemblyScanner
                .FindAuthorizersInAssembly(assembly)
                .ForEach(scanResult => services.AddScanResult(scanResult, lifetime));

            return services;
        }

        /// <summary>
        /// Adds all Authorizers in the assembly of the specified type
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <param name="type">The type whose assembly to scan</param>
        /// <param name="lifetime">The lifetime of the Authorizers. The default is scoped (per-request in web applications)</param>
        /// <param name="filter">Optional filter that allows certain types to be skipped from registration.</param>
        /// <param name="includeInternalTypes">Include internal Authorizers. The default is false.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizersFromAssemblyContaining(this IServiceCollection services, Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            => services.AddAuthorizersFromAssembly(type.Assembly, lifetime);

        /// <summary>
        /// Adds all Authorizers in the assembly of the type specified by the generic parameter
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <param name="lifetime">The lifetime of the Authorizers. The default is scoped (per-request in web applications)</param>
        /// <param name="filter">Optional filter that allows certain types to be skipped from registration.</param>
        /// <param name="includeInternalTypes">Include internal Authorizers. The default is false.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizersFromAssemblyContaining<T>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            => services.AddAuthorizersFromAssembly(typeof(T).Assembly, lifetime);

        /// <summary>
        /// Helper method to register a Authorizer from an AssemblyScanner result
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <param name="scanResult">The scan result</param>
        /// <param name="lifetime">The lifetime of the Authorizers. The default is scoped (per-request in web applications)</param>
        /// <param name="filter">Optional filter that allows certain types to be skipped from registration.</param>
        /// <returns></returns>
        private static IServiceCollection AddScanResult(this IServiceCollection services, AssemblyScanner.AssemblyScanResult scanResult, ServiceLifetime lifetime)
        {
            //Register as interface
            services.Add(
                new ServiceDescriptor(
                    serviceType: scanResult.InterfaceType,
                    implementationType: scanResult.AuthorizerType,
                    lifetime: lifetime));

            //Register as self
            services.Add(
                new ServiceDescriptor(
                    serviceType: scanResult.AuthorizerType,
                    implementationType: scanResult.AuthorizerType,
                    lifetime: lifetime));

            return services;
        }
    }
}
