using Marko.Services.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marko.Services.Impl
{
    public class CarSalesman : ICarSalesman
    {
        private readonly string connectionString;

        public CarSalesman(string connectionString)
        {
            this.connectionString = connectionString;
        }

        async Task<IEnumerable<Car>> ICarSalesman.GetCars()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT id, Manufacturer, Model, Price, Unit, InfoKey, InfoValue FROM allcars ORDER BY Manufacturer, Model, id";

                try
                {
                    await connection.OpenAsync();
                    var result = new List<Car>();

                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetInt32(0);

                        // Do we already have car with this id?
                        var car = result.FirstOrDefault(f => f.Id == id);
                        if (car == null)
                        {
                            // No, create a new one.
                            car = new Car
                            {
                                Id = id,
                                Manufacturer = reader.GetString(1),
                                Model = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                PriceUnit = reader.GetString(4)
                            };

                            result.Add(car);
                        }

                        // Parse all car information, if any
                        if (!reader.IsDBNull(5))
                        {
                            ParseInformation(car, reader.GetString(5), reader.GetString(6));
                        }
                    }

                    return result;
                }
                catch
                {
                    throw new Exception("Real exception hidden for security reasons.");
                }
            }
        }

        async Task<(byte[], string)> ICarSalesman.GetDefaultImage(int carid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Image, ContentType FROM cardefaultpicture WHERE rn = 1 AND carid = @id";
                command.Parameters.AddWithValue("id", carid);

                try
                {
                    await connection.OpenAsync();
                    using var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        return ((byte[])reader[0], (string)reader[1]);
                    }

                    return (null, null);
                }
                catch
                {
                    throw new Exception("Real exception hidden for security reasons.");
                }
            }
        }

        async Task<(byte[], string)> ICarSalesman.GetImage(int pictureid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Image, ContentType FROM carpicture WHERE id = @id";
                command.Parameters.AddWithValue("id", pictureid);

                try
                {
                    await connection.OpenAsync();
                    using var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        return ((byte[])reader[0], (string)reader[1]);
                    }

                    return (null, null);
                }
                catch
                {
                    throw new Exception("Real exception hidden for security reasons.");
                }
            }
        }

        async Task<IEnumerable<int>> ICarSalesman.GetImages(int carid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT id FROM carpictures WHERE carid = @id";
                command.Parameters.AddWithValue("id", carid);

                try
                {
                    await connection.OpenAsync();
                    using var reader = await command.ExecuteReaderAsync();
                    var result = new List<int>();

                    while (await reader.ReadAsync())
                    {
                        result.Add((int)reader[0]);
                    }

                    return result;
                }
                catch
                {
                    throw new Exception("Real exception hidden for security reasons.");
                }
            }
        }

        private void ParseInformation(Car car, string key, string value)
        {
            Debug.Assert(car != null, "Car was null.");
            if (!string.IsNullOrEmpty(key))
            {
                switch (key)
                {
                    case "bhp":
                        car.Horsepower = string.IsNullOrEmpty(value) ? -1 : int.Parse(value);
                        break;
                    case "top speed":
                        car.TopSpeed = string.IsNullOrEmpty(value) ? -1 : int.Parse(value);
                        break;
                    default:
                        Debug.Assert(false, $"Unknown info key '{key}'.");
                        break;
                }
            }
        }
    }
}
