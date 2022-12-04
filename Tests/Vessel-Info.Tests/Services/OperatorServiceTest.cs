namespace Vessel_Info.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models.Operators;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Tests.Common;
    using Xunit;
    
    public class OperatorServiceTest
    {
        private const int operatorId = 1;
        private const string operatorName = "First";

        public OperatorServiceTest()
        {
            MapperInitializer.Initialize();
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectResultWithWhere()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOperator = new Operator { Id = operatorId, Name = operatorName };

            await db.AddAsync(firstOperator);
            await db.SaveChangesAsync();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.GetByIdAsync(operatorId);

            // Assert
            var contentResult = Assert.IsType<OperatorAllServiceModel>(result);

            Assert.Equal(operatorName, contentResult.Name);
            Assert.Equal(operatorId, contentResult.Id);
        }

        [Fact]
        public async Task GetOrCreateOperatorShouldReturnCorrectOperatorIdWhenItIsNotNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOperator = new Operator { Id = operatorId, Name = operatorName };

            await db.AddAsync(firstOperator);
            await db.SaveChangesAsync();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.GetOrCreateOperatorAsync(operatorName);

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(operatorId, contentResult);
        }

        [Fact]
        public async Task GetOrCreateOperatorShouldReturnNewOperatorId()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.GetOrCreateOperatorAsync("SomeName");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(operatorId, contentResult);
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOperator = new Operator { Id = operatorId, Name = operatorName };

            await db.AddAsync(firstOperator);
            await db.SaveChangesAsync();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.DetailsAsync(operatorId);

            // Assert
            var contentResult = Assert.IsType<OperatorDetailsServiceModel>(result);

            Assert.Equal(operatorName, contentResult.Name);
            Assert.Equal(operatorId, contentResult.Id);
        }

        [Fact]
        public void DetailsShouldReturnArgumentNullExceptionWithParameter()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var operatorService = new OperatorService(db);

            // Act
            var exc = Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await operatorService.DetailsAsync(null));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'details')", exc.Result.Message);
        }
    }
}
