using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marko.Utils.Configuration
{
    /// <summary>
    /// Configuration source for downloading DI settings.
    /// </summary>
    /// <remarks>
    /// This class could be altered to download settings from file. Source decision could be made in IConfigurationSource.Build method.
    /// </remarks>
    public class DiConfigurationSource : IConfigurationSource
    {
        private readonly string connectionString;
        private readonly string environment;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="environment">Target environment. For instance "DEBUG".</param>
        public DiConfigurationSource(string connectionString, string environment)
        {
            this.connectionString = connectionString;
            this.environment = environment;
        }

        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder)
        {
            return new DiConfigurationProvider(connectionString, environment);
        }
    }
}
