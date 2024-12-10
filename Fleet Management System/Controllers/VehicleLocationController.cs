using Fleet_Management_System.Dto;
using Fleet_Management_System.Model;
using Fleet_Management_System.RequestModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleLocationController : ControllerBase
    {
        private readonly ILogger<VehicleLocationController> _logger;
        private readonly FleetDbContext _dbContext;
        private readonly IValidator<VehicleLocationRequestModel> _vehicleLocationValidator;

        public VehicleLocationController(FleetDbContext context,
            IValidator<VehicleLocationRequestModel> vehicleLocationValidator,
            ILogger<VehicleLocationController> logger)
        {
            _dbContext = context;
            _vehicleLocationValidator = vehicleLocationValidator;
            _logger = logger;
        }

        // POST /api/vehicles/location
        [HttpPost("location")]
        public async Task<IActionResult> AddVehicleLocation([FromBody] VehicleLocationRequestModel request)
        {
            try
            {
                // Ensure the request is not null
                ArgumentNullException.ThrowIfNull(request, nameof(request));

                // Validate the request using FluentValidation
                var validation = _vehicleLocationValidator.Validate(request);
                if (!validation.IsValid)
                {
                    _logger.LogWarning("Validation failed: {Errors}", validation.Errors);
                    throw new ValidationException(validation.Errors);
                }

                // Check if the vehicle exists in the database
                var vehicle = await _dbContext.Vehicles
                    .FirstOrDefaultAsync(x => x.RegistrationNumber == request.RegistrationNumber);

                if (vehicle is null)
                {
                    // If vehicle doesn't exist, add it to the database
                    vehicle = new Vehicle { RegistrationNumber = request.RegistrationNumber };
                    _dbContext.Vehicles.Add(vehicle);
                    await _dbContext.SaveChangesAsync(); // Save changes to generate VehicleID
                }

                // Create the new VehicleLocation and associate it with the Vehicle
                var location = new VehicleLocation
                {
                    VehicleID = vehicle.VehicleID, // Use the VehicleID from the found or newly added vehicle
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Timestamp = DateTime.UtcNow
                };

                // Add the location to the database
                _dbContext.VehicleLocations.Add(location);
                await _dbContext.SaveChangesAsync(); // Save the new location

                // Return success response
                return Ok("Location added successfully.");
            }
            catch (Exception ex)
            {
                // Log and return error if an exception occurs
                _logger.LogError(ex.Message.Replace("\r\n", "").Trim(), ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // GET /api/vehicles/locations
        [HttpGet("locations")]
        public async Task<IActionResult> GetLatestVehicleLocations()
        {
            try
            {
                var latestLocations = await _dbContext.VehicleLocations
                    .GroupBy(v => v.VehicleID)
                    .Select(g => g.OrderByDescending(v => v.Timestamp)
                      .Select(v => new VehicleLocationDto
                      {
                          VehicleID = v.VehicleID,
                          Latitude = v.Latitude,
                          Longitude = v.Longitude,
                          Timestamp = v.Timestamp,
                          VehicleRegistrationNumber = v.Vehicle.RegistrationNumber
                      }).FirstOrDefault())
                    .ToListAsync();

                return Ok(latestLocations ?? new List<VehicleLocationDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.Replace("\r\n", "").Trim(), ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("all-vehicle-locations")]
        public async Task<IActionResult> GetVehicleLocations()
        {
            try
            {
                var latestLocations = await _dbContext.VehicleLocations
                      .Select(v => new VehicleLocationDto
                      {
                          VehicleID = v.VehicleID,
                          Latitude = v.Latitude,
                          Longitude = v.Longitude,
                          Timestamp = v.Timestamp,
                          VehicleRegistrationNumber = v.Vehicle.RegistrationNumber
                      })
                    .ToListAsync();

                return Ok(latestLocations ?? new List<VehicleLocationDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.Replace("\r\n", "").Trim(), ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}