using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Marko.Utils.Configuration;

namespace TestingTestingProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    // Load DI configurations here. Actual DI magic is done later.
                    builder.AddDiConfiguration(options =>
                    {
                        options.ConnectionString = @"Server=MYLLY; User Id=ConfigReader; Password=ConfigReader;";
                        options.Environment = "DEBUG";
                    });
                });
    }
}
