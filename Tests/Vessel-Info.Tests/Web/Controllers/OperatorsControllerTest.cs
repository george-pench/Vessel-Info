namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Operators;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.Controllers;
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
            var classSocietyService = new Mock<IOperatorService>();

            classSocietyService
                .Setup(cs => cs.DetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new OperatorDetailsServiceModel { Id = operatorId, Name = operatorName });

            var controller = new OperatorsController(classSocietyService.Object);

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
    }
}
