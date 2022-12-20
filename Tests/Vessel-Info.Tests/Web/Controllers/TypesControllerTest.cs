namespace Vessel_Info.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Web.Controllers;
    using Xunit;

    public class TypesControllerTest
    {
        private const int typeId = 1;
        private const string typeName = "SomeTypeName";

        [Fact]
        public async Task AllShouldReturnNotFoundWithInvalidId()
        {
            // Arrange
            var controller = new TypesController(null);

            // Act
            var result = await controller.All(-1);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }
    }
}
