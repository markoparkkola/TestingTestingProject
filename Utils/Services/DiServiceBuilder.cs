using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Marko.Utils.Services
{
    public static class DiServiceBuilder
    {
        private static IConfiguration configurationProvider;

        public static IServiceCollection AddDiServices(this IServiceCollection services, IConfiguration configuration)
        {
            configurationProvider = configuration;

            var section = configuration.GetSection("DiConfiguration");
            foreach (var child in section.GetChildren())
            {
                var type = child.Key;
                foreach (var transient in child.GetChildren())
                {
                    var serviceName = transient.GetSection("Service").Value;
                    var typeName = transient.GetSection("Type").Value;
                    //var ctor = transient.GetSection("Ctor")?.Value;

                    var ctor = CreateConstructor(typeName, transient.GetSection("Ctor")?.Value);

                    switch(type)
                    {
                        case "Transient":
                            services.AddTransient(Type.GetType(serviceName), ctor);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    
                }
            }
            return services;
        }

        private static Func<IServiceProvider, object> CreateConstructor(string typeName, string ctorDefinition)
        {
            var type = Type.GetType(typeName);
            return provider => Activator.CreateInstance(type, CreateParameters(type, ctorDefinition, provider));
        }

        private static object[] CreateParameters(Type type, string ctorDefinition, IServiceProvider provider)
        {
            if (string.IsNullOrEmpty(ctorDefinition))
            {
                return null;
            }

            var parameterDescriptors = ctorDefinition.Split(',');
            var constructors = type.GetConstructors();
            var constructor = constructors.Single(f => f.GetParameters().Length == parameterDescriptors.Length);
            var constructorParameters = constructor.GetParameters();

            return parameterDescriptors
                .Select((descriptor, index) => CreateParameter(descriptor, constructorParameters[index]))
                .ToArray();
        }

        private static object CreateParameter(string descriptor, ParameterInfo parameterInfo)
        {
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
