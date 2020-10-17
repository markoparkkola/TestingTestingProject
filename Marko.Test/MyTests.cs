using Marko.Services;
using Marko.Test.MockHelpers;
using Marko.Utils.Configuration;
using Marko.Utils.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Marko.Test
{
    /// <summary>
    /// This class demonstrates loading test configuration from database, building various kind of mocked types and finally running the rather simple test.
    /// </summary>
    public class MyTests
    {
        private ICarSalesman carSalesman = null;

        [OneTimeSetUp]
        public void Setup()
        {
            // Of course we should test these first but I'm too lazy.
            IConfigurationSource source = new DiConfigurationSource(@"Server=MYLLY; User Id=ConfigReader; Password=ConfigReader;", "TEST");
            IConfigurationProvider provider = source.Build(null);
            provider.Load();

            // Mock these objects.
            var collection = new Mock<ServiceCollectionWrapper>();

            // XXX : This is a mess. Unmess this sometime.
            // We load configuration provided by our DiConfigurationProvider (using DiConfigurationSource as source)
            // and return configurations to caller.
            var diConfiguration = new Mock<IConfigurationSection>();
            diConfiguration.Setup(f => f.GetChildren())
                .Returns(() =>
                {
                    return provider.GetChildKeys(Enumerable.Empty<string>(), "DiConfiguration").Distinct().Select(parentKey =>
                    {
                        var mock = new Mock<IConfigurationSection>();
                        mock.Setup(f => f.Key).Returns(parentKey);
                        mock.Setup(f => f.GetChildren())
                            .Returns(() => provider.GetChildKeys(Enumerable.Empty<string>(), "DiConfiguration:" + parentKey).Distinct().Select(childKey =>
                            {
                                var mock = new Mock<IConfigurationSection>();
                                mock.Setup(f => f.GetSection(It.IsNotNull<string>())).Returns((string section) =>
                                {
                                    var mock = new Mock<IConfigurationSection>();
                                    mock.Setup(f => f.Value).Returns(() => provider.TryGet($"DiConfiguration:{parentKey}:{childKey}:{section}", out string value) ? value : "");
                                    return mock.Object;
                                });
                                return mock.Object;
                            }));
                        return mock.Object;
                    });
                });

            // Main/head configuration. Returns only DI configuration mockup.
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(f => f.GetSection(It.IsAny<string>()))
                .Returns(diConfiguration.Object);

            // Builds DI services and fetches the one and only service we actually need to run the tests.
            // DiServiceBuilder should be tested also before calling it.
            DiServiceBuilder.AddDiServices(collection.Object, configuration.Object);
            carSalesman = collection.Object.GetService(typeof(ICarSalesman)) as ICarSalesman;
        }

        [Test]
        public async Task TestCarListing()
        {
            if (carSalesman == null)
                throw new Exception("Setup must have gone wrong");
            // The test but using our own mocked type now.
            var cars = await carSalesman.GetCars();
            Assert.That(cars.Any(), "GetCars() didn't product any car. Should be three.");
        }
    }
}
