namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Registrations;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Controllers;
    using Xunit;

    public class RegistrationsControllerTest
    {
        private const int registrationId = 1;
        private const string registrationFlagName = "SomeRegistrationFlagName";

        [Fact]
        public async Task AllShouldReturnNotFoundWithInvalidId()
        {
            // Arrange
            var controller = new RegistrationsController(null);

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
            var method = typeof(RegistrationsController).GetMethod(nameof(RegistrationsController.Details));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundWithInvalidRegistrationId()
        {
            // Arrange
            var controller = new RegistrationsController(null);

            // Act
            var result = await controller.Details(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DetailsShouldReturnViewWithCorrectModelAndValidRegistration()
        {
            // Arrange
            var registrationService = new Mock<IRegistrationService>();

            registrationService
               .Setup(cs => cs.DetailsAsync(It.IsAny<int>()))
               .ReturnsAsync(new RegistrationBaseServiceModel { Id = registrationId, Flag = registrationFlagName });

            var controller = new RegistrationsController(registrationService.Object);

            // Act
            var result = await controller.Details(registrationId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<RegistrationBaseServiceModel>().Flag == registrationFlagName);
        }
    }
}
