using Marko.Services;
using Moq;
using NUnit.Framework;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Marko.Test
{
    /// <summary>
    /// This is just a class to demonstrate easy way to test things.
    /// </summary>
    public class MockTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            // Setup test database from a file for instance
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Free any resources
        }

        [Test]
        public async Task TestCarListing()
        {
            // Mock the whole interface
            var carSalesman = new Mock<ICarSalesman>();
            carSalesman.Setup(f => f.GetCars()).Returns(Task.FromResult(Mock.CarSalesman.cars.AsEnumerable()));

            // Do the actual test
            var cars = await carSalesman.Object.GetCars();
            Assert.That(cars.Any(), "GetCars() didn't product any car. Should be three.");
        }
    }
}
