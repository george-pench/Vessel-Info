namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Operators;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Controllers;
    using Vessel_Info.Web.ViewModels.Operators;
    using Xunit;
    
    public class OperatorsControllerTest
    {
        private const int operatorId = 1;
        private const string operatorName = "SomeOperatorName";


        [Fact]
        public async Task AllShouldReturnNotFoundWithInvalidId()
        {
            // Arrange
            var controller = new OperatorsController(null);

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
            var method = typeof(OperatorsController).GetMethod(nameof(OperatorsController.Details));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundWithInvalidOperatorId()
        {
            // Arrange
            var controller = new OperatorsController(null);

            // Act
            var result = await controller.Details(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DetailsShouldReturnViewWithCorrectModelAndValidOperator()
        {
            // Arrange
            var operatorService = new Mock<IOperatorService>();

            operatorService
                .Setup(os => os.DetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new OperatorDetailsServiceModel { Id = operatorId, Name = operatorName });

            var controller = new OperatorsController(operatorService.Object);

            // Act
            var result = await controller.Details(operatorId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<OperatorDetailsServiceModel>().Name == operatorName);
        }

        [Fact]
        public void EditShouldBeForAuthorizedUsersOnly()
        {
            // Arrange
            var method = typeof(OperatorsController)
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
        public async Task EditShouldReturnNotFoundWithInvalidOperatorId()
        {
            // Arrange
            var controller = new OperatorsController(null);

            // Act
            var result = await controller.Edit(null);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task EditShouldReturnViewWithCorrectModelAndValidOperator()
        {
            // Arrange
            var operatorService = new Mock<IOperatorService>();

            operatorService
                .Setup(os => os.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new OperatorAllServiceModel { Id = operatorId, Name = operatorName });

            var controller = new OperatorsController(operatorService.Object);

            // Act
            var result = await controller.Edit(operatorId);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<OperatorAllServiceModel>().Name == operatorName);
        }

        [Fact]
        public void EditShouldHaveHttpPostAttribute()
        {
            // Arrange
            var method = typeof(OperatorsController)
                .GetMethod(nameof(OperatorsController.Edit), new Type[] { typeof(int), typeof(OperatorEditInputModel) });

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
            var controller = new OperatorsController(null);
            controller.ModelState.AddModelError("SessionName", "Required");

            var newSession = new OperatorEditInputModel();

            // Act
            var result = controller.Edit(0, newSession);

            // Assert
            result.Result
               .Should()
               .BeOfType<BadRequestObjectResult>();
        }
    }
}
