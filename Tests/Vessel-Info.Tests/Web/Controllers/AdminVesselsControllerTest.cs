namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Areas.Admin.Controllers;
    using Vessel_Info.Web.ViewModels.Vessels;
    using Xunit;

    public class AdminVesselsControllerTest
    {
        private const string vesselId = "d7f5495a-7cfe-4ef5-8ebe-ec01a23eb22a";
        private const string vesselName = "VesselName";

        [Fact]
        public async Task AllShouldReturnNotFoundWithInvalidId()
        {
            // Arrange
            var controller = new VesselsController(null, null, null, null, null);

            // Act
            var result = await controller.All(-1);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundWithInvalidVesselId()
        {
            // Arrange
            var controller = new VesselsController(null, null, null, null, null);

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

            var controller = new VesselsController(vesselService.Object, null, null, null, null);

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

        [Fact]
        public async Task EditShouldReturnNotFoundWithInvalidClassSocietyId()
        {
            // Arrange
            var controller = new VesselsController(null, null, null, null, null);

            // Act
            var result = await controller.Edit(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task EditShouldReturnViewWithCorrectModelAndValidClassSociety()
        {
            // Arrange
            var vesselService = new Mock<IVesselService>();

            vesselService
                .Setup(cs => cs.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new VesselAllServiceModel { Name = vesselName, Id = vesselId });

            var controller = new VesselsController(vesselService.Object, null, null, null, null);

            // Act
            var result = await controller.Edit(vesselId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<VesselAllServiceModel>().Name == vesselName);
        }

        [Fact]
        public void EditShouldHaveHttpPostAttribute()
        {
            // Arrange
            var method = typeof(VesselsController)
                .GetMethod(nameof(VesselsController.Edit), new Type[] { typeof(string), typeof(VesselEditInputModel) });

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(HttpPostAttribute)));
        }

        [Fact]
        public void EditPostShouldReturnBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new VesselsController(null, null, null, null, null);
            controller.ModelState.AddModelError("SessionName", "Required");

            var newSession = new VesselEditInputModel();

            // Act
            var result = controller.Edit(vesselId, newSession);

            // Assert
            result.Result
               .Should()
               .BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteShouldReturnNotFoundWithInvalidVesselId()
        {
            // Arrange
            var controller = new VesselsController(null, null, null, null, null);

            // Act
            var result = await controller.Delete(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteShouldReturnViewWithCorrectModelAndValidVessel()
        {
            // Arrange
            var vesselService = new Mock<IVesselService>();

            vesselService
               .Setup(cs => cs.GetByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(new VesselAllServiceModel { Id = vesselId, Name = vesselName });

            var controller = new VesselsController(vesselService.Object, null, null, null, null);

            // Act
            var result = await controller.Delete(vesselId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<VesselAllServiceModel>().Name == vesselName);
        }
    }
}
