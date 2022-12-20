namespace Vessel_Info.Tests.Services
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Types;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Tests.Common;
    using Xunit;

    public class TypeServiceTest
    {
        private const int typeId = 1;
        private const string typeName = "First";

        public TypeServiceTest()
        {
            MapperInitializer.Initialize();
        }

        [Fact]
        public async Task GetAllBySearchTermShouldReturnCorrectResultWithWhereAndOrderBy()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstType = new Data.Models.Type { Id = typeId, Name = typeName };
            var secondType = new Data.Models.Type { Id = 2, Name = "SecondType" };
            var thirdType = new Data.Models.Type { Id = 3, Name = "ThirdType" };

            await db.AddRangeAsync(firstType, secondType, thirdType);
            await db.SaveChangesAsync();

            var typeService = new TypeService(db);

            // Act
            var result = typeService.GetAllBySearchTerm("Fi");

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 1)
                .And
                .HaveCount(1);
        }

        [Fact]
        public async Task GetOrCreateTypeShouldReturnCorrectTypeIdWhenItIsNotNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstType = new Data.Models.Type { Id = typeId, Name = typeName };

            await db.AddAsync(firstType);
            await db.SaveChangesAsync();

            var typeService = new TypeService(db);

            // Act
            var result = await typeService.GetOrCreateTypeAsync(typeName);

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(typeId, contentResult);
        }

        [Fact]
        public async Task GetOrCreateTypeShouldReturnNewTypeId()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var typeService = new TypeService(db);

            // Act
            var result = await typeService.GetOrCreateTypeAsync("SomeName");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(typeId, contentResult);
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstType = new Data.Models.Type { Id = typeId, Name = typeName };

            await db.AddAsync(firstType);
            await db.SaveChangesAsync();

            var typeService = new TypeService(db);

            // Act
            var result = await typeService.DetailsAsync(typeId);

            // Assert
            var contentResult = Assert.IsType<TypeBaseServiceModel>(result);

            Assert.Equal(typeName, contentResult.Name);
            Assert.Equal(typeId, contentResult.Id);
        }

        [Fact]
        public void DetailsShouldReturnArgumentNullExceptionWithParameter()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var typeService = new TypeService(db);

            // Act
            var exc = Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await typeService.DetailsAsync(null));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'details')", exc.Result.Message);
        }

        [Fact]
        public async Task FindTypeIdByNameShouldReturnCorrectResultWithWhere()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstType = new Data.Models.Type { Id = typeId, Name = "SomeType" };

            await db.AddAsync(firstType);
            await db.SaveChangesAsync();

            var typeService = new TypeService(db);

            // Act
            var result = await typeService.FindTypeIdByNameAsync("SomeType");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(typeId, contentResult);
        }

        [Fact]
        public void AllShouldReturnCorrectResultWithOrderBy()
        {
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstType = new Data.Models.Type { Id = typeId, Name = typeName };
            var secondType = new Data.Models.Type { Id = 2, Name = "Second" };

            db.AddRange(firstType, secondType);
            db.SaveChanges();

            var typeService = new TypeService(db);

            // Act
            var result = typeService.All();

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

            var firstType = new Data.Models.Type { Id = typeId, Name = typeName };
            var secondType = new Data.Models.Type { Id = 2, Name = "Second" };

            await db.AddRangeAsync(firstType, secondType);
            await db.SaveChangesAsync();

            var typeService = new TypeService(db);

            // Act
            var result = await typeService.GetCountAsync();

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(2, contentResult);
        }
    }
}
