using Marko.Services;
using Marko.Services.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marko.Test.Mock
{
    /// <summary>
    /// Mock type that implements ICarSalesman. Mocking this way
    /// is more efficient than using Moq but now you would have to
    /// test this class also before using it. But there is not much
    /// that could go wrong in this class so I didn't bother.
    /// </summary>
    class CarSalesman : ICarSalesman
    {
        /// <summary>
        /// Public test database of three cars.
        /// </summary>
        public static readonly Car[] cars = new []
        {
            new Car
            {
                Id = 1,
                Manufacturer = "Toyota",
                Model = "Camry",
                Price = 38995.2600M,
                PriceUnit = "eur",
                Horsepower = 214,
                TopSpeed = 180
            },
            new Car
            {
                Id = 1,
                Manufacturer = "Toyota",
                Model = "Corolla",
                Price = 23236.7800M,
                PriceUnit = "eur",
                Horsepower = 122
            },
            new Car
            {
                Id = 1,
                Manufacturer = "Toyota",
                Model = "86",
                Price = 26985.0000M,
                PriceUnit = "usd"
            }
        };

        /// <summary>
        /// Return three cars.
        /// </summary>
        /// <returns>Three cars.</returns>
        Task<IEnumerable<Car>> ICarSalesman.GetCars()
        {
            return Task.FromResult(cars.AsEnumerable());
        }

        /// <summary>
        /// Didn't implement.
        /// </summary>
        Task<(byte[], string)> ICarSalesman.GetDefaultImage(int carid)
        {
            return Task.FromResult((new byte[0], ""));
        }

        /// <summary>
        /// Didn't implement.
        /// </summary>
        Task<(byte[], string)> ICarSalesman.GetImage(int pictureid)
        {
            return Task.FromResult((new byte[0], ""));
        }

        /// <summary>
        /// Didn't implement.
        /// </summary>
        Task<IEnumerable<int>> ICarSalesman.GetImages(int carid)
        {
            return Task.FromResult(Enumerable.Empty<int>());
        }
    }
}
