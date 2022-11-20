namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.ClassSocieties;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Controllers;
    using Vessel_Info.Web.ViewModels.ClassSocieties;
    using Xunit;

    public class ClassSocietiesControllerTest
    {
        private const int classSocietyId = 1;
        private const string classSocietyFullName = "SomeClassSocietyFullName";

        [Fact]
        public async Task AllShouldReturnNotFoundWithInvalidId()
        {
            // Arrange
            var controller = new ClassSocietiesController(null);

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
            var method = typeof(ClassSocietiesController).GetMethod(nameof(ClassSocietiesController.Details));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundWithInvalidClassSocietyId()
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
        public async Task DetailsShouldReturnViewWithCorrectModelAndValidClassSociety()
        {
            // Arrange
            var classSocietyService = new Mock<IClassificationSocietyService>();

            classSocietyService
               .Setup(cs => cs.DetailsAsync(It.IsAny<int>())) 
               .ReturnsAsync(new ClassSocietyDetailsServiceModel { Id =  classSocietyId, FullName = classSocietyFullName });

            var controller = new ClassSocietiesController(classSocietyService.Object);

            // Act
            var result = await controller.Details(classSocietyId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<ClassSocietyDetailsServiceModel>().FullName == classSocietyFullName);
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

        [Fact]
        public void EditShouldHaveHttpPostAttribute()
        {
            // Arrange
            var method = typeof(ClassSocietiesController)
                .GetMethod(nameof(ClassSocietiesController.Edit), new Type[] { typeof(int), typeof(ClassSocietyEditInputModel) });

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
            var controller = new ClassSocietiesController(null);
            controller.ModelState.AddModelError("SessionName", "Required");

            var newSession = new ClassSocietyEditInputModel();

            // Act
            var result = controller.Edit(0, newSession);

            // Assert
            result.Result
               .Should()
               .BeOfType<BadRequestObjectResult>();
        }
    }
}
