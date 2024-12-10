using Fleet_Management_System.Dto;
using FleetManagement_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FleetManagement_FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return File("/frontend/index.html", "text/html");
        }

        public async Task<IActionResult> AllVehicles()
        {
            var apiUrl = "https://localhost:44361/api/vehicles/all-vehicle-locations";
            List<VehicleLocationDto> vehicles = new();

            try
            {
                // Fetch data from the backend API
                vehicles = await _httpClient.GetFromJsonAsync<List<VehicleLocationDto>>(apiUrl);
            }
            catch (HttpRequestException ex)
            {
                // Log the error or handle it
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }

            // Pass data to the strongly-typed view
            return View(vehicles);
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
    }
}