using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Marko.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestingTestingProject.Models;

namespace TestingTestingProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICarSalesman carSalesman;

        // DI magic can be seen here. See above we have introduced instance variable called 'carSalesman'.
        // It is null until we give it value here on the class constructor. But where does the parameter comes from?
        // Well, we configured our little DI service builder in Startup.cs file, remember?
        public HomeController(ILogger<HomeController> logger, ICarSalesman carSalesman)
        {
            this.logger = logger;
            this.carSalesman = carSalesman;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<JsonResult> GetCars()
        {
            // Because services are loaded by magic, we don't need ugly database spells here.
            // Let our async ICarSalesman interface do the job.
            // One could do all the assertions and checking from browser provided parameters here, if there were any,
            // or possibly map view models to business models (and vice versa).
            return Json(await carSalesman.GetCars());
        }

        [HttpGet]
        public async Task<FileResult> GetDefaultImage(int id)
        {
            var (bytes, contentType) = await carSalesman.GetDefaultImage(id);
            if (bytes == null || contentType == null)
            {
                return null;
            }

            return File(bytes, contentType);
        }

        [HttpGet]
        public async Task<JsonResult> GetImages(int id)
        {
            return Json(await carSalesman.GetImages(id));
        }

        [HttpGet]
        public async Task<FileResult> GetImage(int id)
        {
            var (bytes, contentType) = await carSalesman.GetImage(id);
            if (bytes == null || contentType == null)
            {
                return null;
            }

            return File(bytes, contentType);
        }
    }
}
