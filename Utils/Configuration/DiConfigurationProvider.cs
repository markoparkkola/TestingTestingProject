using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Marko.Utils.Configuration
{
    internal class DiConfigurationProvider : ConfigurationProvider
    {
        private readonly string connectionString;

        public DiConfigurationProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            string target = "DEBUG"; // RELEASE, TEST, etc.
            command.CommandText = @$"
SELECT [Key], [Value]
FROM [Configuration] c
JOIN [Source] s ON s.id = c.sourceid
JOIN [Target] t ON t.id = c.targetid
WHERE s.[Name] = 'TestingTesting' AND t.[Name] = '{target}'";

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Set(reader["Key"] as string, reader["Value"] as string);
            }
        }
    }
}