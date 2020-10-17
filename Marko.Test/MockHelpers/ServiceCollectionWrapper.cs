using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marko.Test.MockHelpers
{
    /// <summary>
    /// Helper class to mock IServiceCollection. IServiceCollection uses a lot of static methods and
    /// those can't be easily mocked.
    /// </summary>
    public class ServiceCollectionWrapper : List<ServiceDescriptor>, IServiceCollection, IServiceProvider
    {
        /// <summary>
        /// For singleton services.
        /// </summary>
        private readonly IDictionary<Type, object> instantiatedServices = new Dictionary<Type, object>();

        public object GetService(Type serviceType)
        {
            var service = this.FirstOrDefault(f => f.ServiceType == serviceType);
            if (service == null)
            {
                return service;
            }

            switch(service.Lifetime)
            {
                case ServiceLifetime.Scoped:
                case ServiceLifetime.Transient:
                    return service.ImplementationFactory(this);
                case ServiceLifetime.Singleton:
                    if (instantiatedServices.TryGetValue(serviceType, out object value))
                    {
                        return value;
                    }
                    instantiatedServices[serviceType] = service.ImplementationFactory(this);
                    return instantiatedServices[serviceType];
                default:
                    throw new Exception();
            }
        }
    }
}
