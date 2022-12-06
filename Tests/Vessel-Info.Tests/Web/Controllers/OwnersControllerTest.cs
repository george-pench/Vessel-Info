namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Owners;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Controllers;
    using Vessel_Info.Web.ViewModels.Owners;
    using Xunit;

    public class OwnersControllerTest
    {
        private const int ownerId = 1;
        private const string ownerName = "SomeOwnerName";

        [Fact]
        public async Task AllShouldReturnNotFoundWithInvalidId()
        {
            // Arrange
            var controller = new OwnersController(null);

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
            var method = typeof(OwnersController).GetMethod(nameof(OwnersController.Details));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundWithInvalidOwnerId()
        {
            // Arrange
            var controller = new ClassSocietiesController(null);

            // Act
            var result = await controller.Details(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DetailsShouldReturnViewWithCorrectModelAndValidOwner()
        {
            // Arrange
            var ownerService = new Mock<IOwnerService>();

            ownerService
               .Setup(cs => cs.DetailsAsync(It.IsAny<int>()))
               .ReturnsAsync(new OwnerDetailsServiceModel { Id = ownerId, Name = ownerName });

            var controller = new OwnersController(ownerService.Object);

            // Act
            var result = await controller.Details(ownerId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<OwnerDetailsServiceModel>().Name == ownerName);
        }

        [Fact]
        public void EditShouldBeForAuthorizedUsersOnly()
        {
            // Arrange
            var method = typeof(OwnersController)
                .GetMethods()
                .Where(m => m.Name == "Edit")
                .FirstOrDefault();

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task EditShouldReturnNotFoundWithInvalidOwnerId()
        {
            // Arrange
            var controller = new OwnersController(null);

            // Act
            var result = await controller.Edit(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task EditShouldReturnViewWithCorrectModelAndValidOwner()
        {
            // Arrange
            var ownerService = new Mock<IOwnerService>();

            ownerService
                .Setup(cs => cs.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new OwnerAllServiceModel { Name = ownerName, Id = ownerId });

            var controller = new OwnersController(ownerService.Object);

            // Act
            var result = await controller.Edit(ownerId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<OwnerAllServiceModel>().Name == ownerName);
        }

        [Fact]
        public void EditShouldHaveHttpPostAttribute()
        {
            // Arrange
            var method = typeof(OwnersController)
                .GetMethod(nameof(OwnersController.Edit), new Type[] { typeof(int), typeof(OwnerEditInputModel) });

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
            var controller = new OwnersController(null);
            controller.ModelState.AddModelError("SessionName", "Required");

            var newSession = new OwnerEditInputModel();

            // Act
            var result = controller.Edit(0, newSession);

            // Assert
            result.Result
               .Should()
               .BeOfType<BadRequestObjectResult>();
        }
    }
}
