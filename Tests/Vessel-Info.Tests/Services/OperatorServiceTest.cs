namespace Vessel_Info.Tests.Services
{
    using FluentAssertions;
    using System;
    using System.Linq;
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

        [Fact]
        public async Task EditShouldReturnFalseWhenModelIsNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.EditAsync(null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOperator = new Operator { Id = operatorId, Name = operatorName };
            var secondOperator = new OperatorEditServiceModel { Id = 2, Name = "Second" };

            await db.AddAsync(firstOperator);
            await db.SaveChangesAsync();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.EditAsync(operatorId, secondOperator);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllBySearchTermShouldReturnCorrectResultWithWhereAndOrderBy()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOperator = new Operator { Id = operatorId, Name = operatorName };
            var secondOperator = new Operator { Id = 2, Name = "Second" };
            var thirdOperator = new Operator { Id = 3, Name = "Third" };

            await db.AddRangeAsync(firstOperator, secondOperator, thirdOperator);
            await db.SaveChangesAsync();

            var operatorService = new OperatorService(db);

            // Act
            var result = operatorService.GetAllBySearchTerm("Fi");

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 1)
                .And
                .HaveCount(1);
        }

        [Fact]
        public async Task FindOperatorIdByNameShouldReturnCorrectResultWithWhere()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var operatorClass = new Operator { Id = operatorId, Name = "SomeOperator" };

            await db.AddAsync(operatorClass);
            await db.SaveChangesAsync();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.FindOperatorIdByNameAsync("SomeOperator");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(operatorId, contentResult);
        }

        [Fact]
        public void AllShouldReturnCorrectResultWithOrderBy()
        {
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOperator = new Operator { Id = operatorId, Name = operatorName };
            var secondOperator = new Operator { Id = 2, Name = "Second" };

            db.AddRange(firstOperator, secondOperator);
            db.SaveChanges();

            var operatorService = new OperatorService(db);

            // Act
            var result = operatorService.All();

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 1)
                .And
                .HaveCount(2);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOperator = new Operator { Id = operatorId, Name = operatorName };
            var secondOperator = new Operator { Id = 2, Name = "Second" };

            await db.AddRangeAsync(firstOperator, secondOperator);
            await db.SaveChangesAsync();

            var operatorService = new OperatorService(db);

            // Act
            var result = await operatorService.GetCountAsync();

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(2, contentResult);
        }
    }
}
