using Microsoft.Extensions.Configuration;
using System;

namespace Marko.Utils.Configuration
{
    public static class DiConfigurationExtensions
    {
        public static IConfigurationBuilder AddDiConfiguration(this IConfigurationBuilder builder, Action<DiConfigurationOptions> options)
        {
            var tmp = new DiConfigurationOptions();
            options?.Invoke(tmp);
            builder.Add(new DiConfigurationSource(tmp.ConnectionString));
            return builder;
        }
    }
}
