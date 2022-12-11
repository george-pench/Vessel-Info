namespace Vessel_Info.Tests.Services
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models.Owners;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Tests.Common;
    using Xunit;

    public class OwnerServiceTest
    {
        private const int ownerId = 1;
        private const string ownerName = "First";

        public OwnerServiceTest()
        {
            MapperInitializer.Initialize();
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectResultWithWhere()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOwner = new Owner { Id = ownerId, Name = ownerName };

            await db.AddAsync(firstOwner);
            await db.SaveChangesAsync();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.GetByIdAsync(ownerId);

            // Assert
            var contentResult = Assert.IsType<OwnerAllServiceModel>(result);

            Assert.Equal(ownerName, contentResult.Name);
            Assert.Equal(ownerId, contentResult.Id);
        }

        [Fact]
        public async Task GetOrCreateOwnerShouldReturnCorrectOwnerIdWhenItIsNotNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOwner = new Owner { Id = ownerId, Name = ownerName };

            await db.AddAsync(firstOwner);
            await db.SaveChangesAsync();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.GetOrCreateOwnerAsync(ownerName);

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(ownerId, contentResult);
        }

        [Fact]
        public async Task GetOrCreateOwnerShouldReturnNewOwnerId()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.GetOrCreateOwnerAsync("SomeName");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(ownerId, contentResult);
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOwner = new Owner { Id = ownerId, Name = ownerName };

            await db.AddAsync(firstOwner);
            await db.SaveChangesAsync();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.DetailsAsync(ownerId);

            // Assert
            var contentResult = Assert.IsType<OwnerDetailsServiceModel>(result);

            Assert.Equal(ownerName, contentResult.Name);
            Assert.Equal(ownerId, contentResult.Id);
        }

        [Fact]
        public void DetailsShouldReturnArgumentNullExceptionWithParameter()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var ownerService = new OwnerService(db);

            // Act
            var exc = Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await ownerService.DetailsAsync(null));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'details')", exc.Result.Message);
        }

        [Fact]
        public async Task EditShouldReturnFalseWhenModelIsNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.EditAsync(null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOwner = new Owner { Id = ownerId, Name = ownerName };
            var secondOwner = new OwnerEditServiceModel { Id = 2, Name = "Second" };

            await db.AddAsync(firstOwner);
            await db.SaveChangesAsync();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.EditAsync(ownerId, secondOwner);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllBySearchTermShouldReturnCorrectResultWithWhereAndOrderBy()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOwner = new Owner { Id = ownerId, Name = ownerName };
            var secondOwner = new Owner { Id = 2, Name = "Second" };
            var thirdOwner = new Owner { Id = 3, Name = "Third" };

            await db.AddRangeAsync(firstOwner, secondOwner, thirdOwner);
            await db.SaveChangesAsync();

            var ownerService = new OwnerService(db);

            // Act
            var result = ownerService.GetAllBySearchTerm("Fi");

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 1)
                .And
                .HaveCount(1);
        }

        [Fact]
        public async Task FindOwnerIdByNameShouldReturnCorrectResultWithWhere()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOwner = new Owner { Id = ownerId, Name = "SomeOwner" };

            await db.AddAsync(firstOwner);
            await db.SaveChangesAsync();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.FindOwnerIdByNameAsync("SomeOwner");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(ownerId, contentResult);
        }

        [Fact]
        public void AllShouldReturnCorrectResultWithOrderBy()
        {
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstOwner = new Owner { Id = ownerId, Name = ownerName };
            var secondOwner = new Owner { Id = 2, Name = "Second" };

            db.AddRange(firstOwner, secondOwner);
            db.SaveChanges();

            var ownerService = new OwnerService(db);

            // Act
            var result = ownerService.All();

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

            var firstOwner = new Owner { Id = ownerId, Name = ownerName };
            var secondOwner = new Owner { Id = 2, Name = "Second" };

            await db.AddRangeAsync(firstOwner, secondOwner);
            await db.SaveChangesAsync();

            var ownerService = new OwnerService(db);

            // Act
            var result = await ownerService.GetCountAsync();

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(2, contentResult);
        }
    }
}
