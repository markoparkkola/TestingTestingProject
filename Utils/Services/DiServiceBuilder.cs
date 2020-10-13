using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Marko.Utils.Services
{
    /// <summary>
    /// Class to load dependencies based on settings in "DiConfiguration" section.
    /// </summary>
    public static class DiServiceBuilder
    {
        private static IConfiguration configurationProvider;

        /// <summary>
        /// Extension method to load dependencies as services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration instance.</param>
        /// <returns>Service collection.</returns>
        /// <example>
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///    services.AddDiServices(Configuration);
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// Settings are stored in the following form to the database:
        /// <code>
        /// Key | Value
        /// DiConfiguration:Transient:0:Service | Marko.Services.ICarSalesman, Marko.Services
        /// DiConfiguration:Transient:0:Type | Marko.Services.Impl.CarSalesman, Marko.Services
        /// DiConfiguration:Transient:0:Ctor | @Default
        /// </code>
        /// Ctor is optional.
        /// @ returns the connection string with the name after the @ character (so @Default returns connection string named "Default").
        /// </remarks>
        public static IServiceCollection AddDiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Store reference to static variable. We'll be needing this later. (yes, statics are bad, mmmkay)
            configurationProvider = configuration;

            var section = configuration.GetSection("DiConfiguration");
            foreach (var child in section.GetChildren())
            {
                // What kind of service this is (transient, scoped, singleton).
                var type = child.Key;
                foreach (var setting in child.GetChildren())
                {
                    // Get service (interface) name and implementing type
                    // Create constructor to active implementing type
                    var serviceName = setting.GetSection("Service").Value;
                    var typeName = setting.GetSection("Type").Value;
                    var ctor = CreateConstructor(typeName, setting.GetSection("Ctor")?.Value);

                    switch (type)
                    {
                        case "Transient":
                            services.AddTransient(Type.GetType(serviceName), ctor);
                            break;
                        default:
                            // XXX : Implement scoped and singleton services some day
                            throw new NotSupportedException();
                    }

                }
            }
            return services;
        }

        /// <summary>
        /// Creates constructor for type, or actually a delegate method which acts as constructor.
        /// </summary>
        /// <param name="typeName">Name of the type we are constructing.</param>
        /// <param name="ctorDefinition">Constructor parameters.</param>
        /// <returns>Delegate for the constructor.</returns>
        private static Func<IServiceProvider, object> CreateConstructor(string typeName, string ctorDefinition)
        {
            var type = Type.GetType(typeName);
            return provider => Activator.CreateInstance(type, CreateParameters(type, ctorDefinition, provider));
        }

        /// <summary>
        /// Creates parameters for the constructor using our own little language.
        /// </summary>
        /// <param name="type">Type of the object we are creating constructor and parameters for.</param>
        /// <param name="ctorDefinition">Constructor parameters.</param>
        /// <param name="provider">Framework provided provider for accessing other services at run time.</param>
        /// <returns>Array of parameters for the constructor.</returns>
        private static object[] CreateParameters(Type type, string ctorDefinition, IServiceProvider provider)
        {
            // If there is no parameters, we can return null and default constructor is used.
            if (string.IsNullOrEmpty(ctorDefinition))
            {
                return null;
            }

            // Try to find constructor that has as many parameters as provided in ctorDefinition parameter
            var parameterDescriptors = ctorDefinition.Split(',');
            var constructors = type.GetConstructors();
            var constructor = constructors.Single(f => f.GetParameters().Length == parameterDescriptors.Length);
            var constructorParameters = constructor.GetParameters();

            // Finally parse all the parameters together into one array (to rule them all)
            return parameterDescriptors
                .Select((descriptor, index) => CreateParameter(descriptor, constructorParameters[index]))
                .ToArray();
        }

        /// <summary>
        /// Return a single parameter for constructor.
        /// </summary>
        /// <param name="descriptor">Parameter descriptor.</param>
        /// <param name="parameterInfo"><see cref="System.Reflection.ParameterInfo"/></param>
        /// <returns>Paramater as object type.</returns>
        private static object CreateParameter(string descriptor, ParameterInfo parameterInfo)
        {
            // we support only connection strings currently
            if (descriptor.StartsWith('@'))
            {
                // connection string
                if (parameterInfo.ParameterType != typeof(string))
                {
                    throw new InvalidCastException();
                }

                return configurationProvider.GetConnectionString(descriptor.Substring(1));
            }

            return null;
        }
    }
}
