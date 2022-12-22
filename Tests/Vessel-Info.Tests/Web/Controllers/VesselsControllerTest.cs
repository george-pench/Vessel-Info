namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Controllers;
    using Xunit;

    public class VesselsControllerTest
    {
        private const string vesselId = "d7f5495a-7cfe-4ef5-8ebe-ec01a23eb22a";
        private const string vesselName = "VesselName";

        [Fact]
        public async Task AllShouldReturnNotFoundWithInvalidId()
        {
            // Arrange
            var controller = new VesselsController(null);

            // Act
            var result = await controller.All(-1);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }


        [Fact]
        public void DetailsShouldBeForAuthorizedUsersOnly()
        {
            // Arrange
            var method = typeof(VesselsController).GetMethod(nameof(VesselsController.Details));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundWithInvalidVesselId()
        {
            // Arrange
            var controller = new VesselsController(null);

            // Act
            var result = await controller.Details(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DetailsShouldReturnViewWithCorrectModelAndValidVessel()
        {
            // Arrange
            var vesselService = new Mock<IVesselService>();

            vesselService
               .Setup(cs => cs.DetailsAsync(It.IsAny<string>()))
               .ReturnsAsync(new VesselDetailsServiceModel { Id = vesselId, Name = vesselName });

            var controller = new VesselsController(vesselService.Object);

            // Act
            var result = await controller.Details(vesselId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<VesselDetailsServiceModel>().Name == vesselName);
        }
    }
}
