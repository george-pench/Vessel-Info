namespace Vessel_Info.Tests.Services
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models.ClassSocieties;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Tests.Common;
    using Xunit;

    public class ClassificationSocietyServiceTest
    {
        // TODO: AAA patern
        private const int classSocietyId = 1;
        private const string classSocietyFullName = "First";
        
        public ClassificationSocietyServiceTest()
        {
            MapperInitializer.Initialize();
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectResultWithWhere()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = classSocietyFullName };
            
            await db.AddAsync(firstClass);
            await db.SaveChangesAsync();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.GetByIdAsync(classSocietyId);

            // Assert
            var contentResult = Assert.IsType<ClassSocietyAllServiceModel>(result);

            Assert.Equal(classSocietyFullName, contentResult.FullName);
            Assert.Equal(classSocietyId, contentResult.Id);
        }

        [Fact]
        public async Task GetOrCreateClassSocietyShouldReturnCorrectClassSocietyIdWhenItIsNotNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = classSocietyFullName };

            await db.AddAsync(firstClass);
            await db.SaveChangesAsync();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.GetOrCreateClassSocietyAsync(classSocietyFullName);

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(classSocietyId, contentResult);
        }

        [Fact]
        public async Task GetOrCreateClassSocietyShouldReturnNewClassSocietyId()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();
            
            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.GetOrCreateClassSocietyAsync("SomeName");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(classSocietyId, contentResult);
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = classSocietyFullName };

            await db.AddAsync(firstClass);
            await db.SaveChangesAsync();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.DetailsAsync(classSocietyId);

            // Assert
            var contentResult = Assert.IsType<ClassSocietyDetailsServiceModel>(result);

            Assert.Equal(classSocietyFullName, contentResult.FullName);
            Assert.Equal(classSocietyId, contentResult.Id);
        }

        [Fact]
        public void DetailsShouldReturnArgumentNullExceptionWithParameter()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var exc = Assert.ThrowsAsync<ArgumentNullException>(async () 
                => await classSocietyService.DetailsAsync(null));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'details')", exc.Result.Message);
        }

        [Fact]
        public async Task EditShouldReturnFalseWhenModelIsNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.EditAsync(null, null);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = classSocietyFullName };
            var secondClass = new ClassSocietyEditServiceModel { Id = 2, FullName = "Second" };

            await db.AddAsync(firstClass);
            await db.SaveChangesAsync();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.EditAsync(classSocietyId, secondClass);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllBySearchTermShouldReturnCorrectResultWithWhereAndOrderBy()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = classSocietyFullName };
            var secondClass = new ClassificationSociety { Id = 2, FullName = "Second" };
            var thirdClass = new ClassificationSociety { Id = 3, FullName = "Third" };

            await db.AddRangeAsync(firstClass, secondClass, thirdClass);
            await db.SaveChangesAsync();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = classSocietyService.GetAllBySearchTerm("Fi");

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 1)
                .And
                .HaveCount(1);
        }

        [Fact]
        public async Task FindClassSocietyIdByNameShouldReturnCorrectResultWithWhere()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = "SomeClass" };

            await db.AddAsync(firstClass);
            await db.SaveChangesAsync();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.FindClassSocietyIdByNameAsync("SomeClass");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(classSocietyId, contentResult);
        }

        [Fact]
        public void AllShouldReturnCorrectResultWithOrderBy()
        {
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = classSocietyFullName };
            var secondClass = new ClassificationSociety { Id = 2, FullName = "Second" };

            db.AddRange(firstClass, secondClass);
            db.SaveChanges();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = classSocietyService.All();

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

            var firstClass = new ClassificationSociety { Id = classSocietyId, FullName = classSocietyFullName };
            var secondClass = new ClassificationSociety { Id = 2, FullName = "Second" };

            await db.AddRangeAsync(firstClass, secondClass);
            await db.SaveChangesAsync();

            var classSocietyService = new ClassificationSocietyService(db);

            // Act
            var result = await classSocietyService.GetCountAsync();

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(2, contentResult);
        }
    }
}
