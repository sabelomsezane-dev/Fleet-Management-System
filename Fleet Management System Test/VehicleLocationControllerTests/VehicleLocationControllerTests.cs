using Xunit;
using Microsoft.AspNetCore.Mvc;
using Fleet_Management_System.Controllers;
using Fleet_Management_System.Model;
using Fleet_Management_System_Test.Helpers;
using Fleet_Management_System.RequestModels;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Microsoft.Extensions.Logging;
using Fleet_Management_System.Dto;

namespace Fleet_Management_System_Test.VehicleLocationControllerTests
{
    public class VehicleLocationControllerTests
    {
        [Fact]
        public async Task AddVehicleLocation_ShouldReturnOk_WhenDataIsValid()
        {
            // Arrange: Create the test DbContext and mock validator
            var dbContext = TestDbContextFactory.CreateTestDbContext();
            var mockValidator = new Mock<IValidator<VehicleLocationRequestModel>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<VehicleLocationRequestModel>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            var controller = new VehicleLocationController(dbContext, mockValidator.Object, new NullLogger<VehicleLocationController>());

            var request = new VehicleLocationRequestModel
            {
                RegistrationNumber = "AB 0001 GP",
                Latitude = 40.7128,
                Longitude = -74.0060
            };

            // Act: Call the AddVehicleLocation method
            var result = await controller.AddVehicleLocation(request);

            // Assert: Verify the result is an OkObjectResult
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLatestVehicleLocations_ShouldReturnEmptyList_WhenNoData()
        {
            // Arrange: Create the test DbContext and controller
            var dbContext = TestDbContextFactory.CreateTestDbContext();
            var mockValidator = new Mock<IValidator<VehicleLocationRequestModel>>();
            var mockLogger = Mock.Of<ILogger<VehicleLocationController>>();
            var controller = new VehicleLocationController(dbContext, mockValidator.Object, mockLogger);

            // Act: Call the GetLatestVehicleLocations method
            var result = await controller.GetLatestVehicleLocations();

            // Assert: Verify the result is an OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as List<VehicleLocationDto>;

            // Verify the result is not null and empty
            Assert.NotNull(value);
            Assert.Empty(value);
        }
    }
}