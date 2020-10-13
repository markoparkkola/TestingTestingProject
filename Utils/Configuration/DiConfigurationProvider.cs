using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters;

namespace Marko.Utils.Configuration
{
    /// <summary>
    /// Loads DI configurations from database.
    /// </summary>
    internal class DiConfigurationProvider : ConfigurationProvider
    {
        private readonly string connectionString;
        private readonly string environment;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">Connection string of the database.</param>
        public DiConfigurationProvider(string connectionString, string environment)
        {
            this.connectionString = connectionString;
            this.environment = environment;
        }

        /// <summary>
        /// Called by the framework to load settings.
        /// </summary>
        public override void Load()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            command.CommandText = @$"
SELECT [Key], [Value]
FROM [Configuration] c
JOIN [Source] s ON s.id = c.sourceid
JOIN [Target] t ON t.id = c.targetid
WHERE s.[Name] = 'TestingTesting' AND t.[Name] = @env";
            command.Parameters.AddWithValue("env", environment);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Set(reader["Key"] as string, reader["Value"] as string);
            }
        }
    }
}