namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.ClassSocieties;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Controllers;
    using Xunit;

    public class ClassSocietiesControllerTest
    {
        [Fact]
        public void DetailsShouldBeForAuthorizedUsersOnly()
        {
            // Arrange
            var method = typeof(ClassSocietiesController).GetMethod(nameof(ClassSocietiesController.Details));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public void EditShouldBeForAuthorizedUsersOnly()
        {
            // Arrange
            var method = typeof(ClassSocietiesController)
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
        public async Task EditShouldReturnNotFoundWithInvalidClassSocietyId()
        {
            // Arrange
            var controller = new ClassSocietiesController(null);

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
            int classSocietyId = 79;
            const string classSocietyFullName = "SomeClassSocietyFullName";

            var classSocietyService = new Mock<IClassificationSocietyService>();
            
            classSocietyService
                .Setup(cs => cs.GetByIdAsync(It.IsAny<int>())) // It.Is<int>(id => id == classSocietyId)
                .ReturnsAsync(new ClassSocietyAllServiceModel { FullName = classSocietyFullName, Id = classSocietyId });
            
            var controller = new ClassSocietiesController(classSocietyService.Object);

            // Act
            var result = await controller.Edit(classSocietyId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<ClassSocietyAllServiceModel>().FullName == classSocietyFullName);
        }
    }
}
