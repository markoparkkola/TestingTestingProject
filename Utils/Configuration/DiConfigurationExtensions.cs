using Microsoft.Extensions.Configuration;
using System;

namespace Marko.Utils.Configuration
{
    /// <summary>
    /// Extension class to load DI configurations.
    /// </summary>
    public static class DiConfigurationExtensions
    {
        /// <summary>
        /// Extension method to load DI configurations.
        /// </summary>
        /// <param name="builder">Configuration builder instance.</param>
        /// <param name="options">Options. Required.</param>
        /// <returns>Configuration builder instance.</returns>
        /// <example>
        /// <code>
        /// public static IHostBuilder CreateHostBuilder(string[] args) =>
        ///     Host
        ///         .CreateDefaultBuilder(args)
        ///         .ConfigureWebHostDefaults(webBuilder =>
        ///         {
        ///             webBuilder.UseStartup<Startup>();
        ///         })
        ///         .ConfigureAppConfiguration((context, builder) =>
        ///         {
        ///             builder.AddDiConfiguration(options => 
        ///             {
        ///                options.ConnectionString = @"Server=MYLLY; User Id=ConfigReader; Password=ConfigReader;";
        ///                options.Environment = "DEBUG";
        ///            });
        ///         });
        ///</code>
        /// </example>
        public static IConfigurationBuilder AddDiConfiguration(this IConfigurationBuilder builder, Action<DiConfigurationOptions> options)
        {
            var tmp = new DiConfigurationOptions();
            options.Invoke(tmp);
            builder.Add(new DiConfigurationSource(tmp.ConnectionString, tmp.Environment));
            return builder;
        }
    }
}
