using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marko.Utils.Configuration
{
    public class DiConfigurationSource : IConfigurationSource
    {
        private readonly string connectionString;

        public DiConfigurationSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder)
        {
            return new DiConfigurationProvider(connectionString);
        }
    }
}
