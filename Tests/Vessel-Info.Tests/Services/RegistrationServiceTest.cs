namespace Vessel_Info.Tests.Services
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models.Registrations;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Tests.Common;
    using Xunit;

    public class RegistrationServiceTest
    {
        private const int registrationId = 1;
        private const string registrationFlagName = "FirstFlag";
        private const string registrationPortName = "FirstPort";

        public RegistrationServiceTest()
        {
            MapperInitializer.Initialize();
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectResultWithWhere()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstRegistration = new Registration { Id = registrationId, Flag = registrationFlagName };

            await db.AddAsync(firstRegistration);
            await db.SaveChangesAsync();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.GetByIdAsync(registrationId);

            // Assert
            var contentResult = Assert.IsType<RegistrationBaseServiceModel>(result);

            Assert.Equal(registrationFlagName, contentResult.Flag);
            Assert.Equal(registrationId, contentResult.Id);
        }

        [Fact]
        public async Task GetOrCreateRegistrationShouldReturnCorrectRegistrationIdWhenItIsNotNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstRegistration = new Registration { Id = registrationId, Flag = registrationFlagName };

            await db.AddAsync(firstRegistration);
            await db.SaveChangesAsync();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.GetOrCreateRegistrationAsync(registrationFlagName, registrationPortName);

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(registrationId, contentResult);
        }

        [Fact]
        public async Task GetOrCreateRegistrationShouldReturnNewRegistrationId()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.GetOrCreateRegistrationAsync("SomeFlagName", "SomePortName");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(registrationId, contentResult);
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstRegistration = new Registration { Id = registrationId, Flag = registrationFlagName };

            await db.AddAsync(firstRegistration);
            await db.SaveChangesAsync();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.DetailsAsync(registrationId);

            // Assert
            var contentResult = Assert.IsType<RegistrationBaseServiceModel>(result);

            Assert.Equal(registrationFlagName, contentResult.Flag);
            Assert.Equal(registrationId, contentResult.Id);
        }

        [Fact]
        public void DetailsShouldReturnArgumentNullExceptionWithParameter()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var registrationService = new RegistrationService(db);

            // Act
            var exc = Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await registrationService.DetailsAsync(null));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'details')", exc.Result.Message);
        }

        [Fact]
        public async Task EditShouldReturnFalseWhenModelIsNull()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.EditAsync(null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditShouldReturnCorrectResult()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstRegistration = new Registration { Id = registrationId, Flag = registrationFlagName };
            var secondRegistration = new RegistrationBaseServiceModel { Id = 2, Flag = "SecondFlag" };

            await db.AddAsync(firstRegistration);
            await db.SaveChangesAsync();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.EditAsync(registrationId, secondRegistration);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllBySearchTermShouldReturnCorrectResultWithWhereAndOrderBy()
        {
            // Arrange           
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstRegistration = new Registration { Id = registrationId, Flag = registrationFlagName };
            var secondRegistration = new Registration { Id = 2, Flag = "SecondFlag" };
            var thirdRegistration = new Registration { Id = 3, Flag = "ThirdFlag" };

            await db.AddRangeAsync(firstRegistration, secondRegistration, thirdRegistration);
            await db.SaveChangesAsync();

            var registrationService = new RegistrationService(db);

            // Act
            var result = registrationService.GetAllBySearchTerm("Fi");

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 1)
                .And
                .HaveCount(1);
        }

        [Fact]
        public async Task FindRegistrationIdByNameShouldReturnCorrectResultWithWhere()
        {
            // Arrange
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstRegistration = new Registration { Id = registrationId, Flag = "SomeFlag" };

            await db.AddAsync(firstRegistration);
            await db.SaveChangesAsync();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.FindRegistrationIdByNameAsync("SomeFlag");

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(registrationId, contentResult);
        }

        [Fact]
        public void AllShouldReturnCorrectResultWithOrderBy()
        {
            var db = VesselInfoDbContextInMemory.GetDatabase();

            var firstRegistration = new Registration { Id = registrationId, Flag = registrationFlagName };
            var secondRegistration = new Registration { Id = 2, Flag = "SecondFlag" };

            db.AddRange(firstRegistration, secondRegistration);
            db.SaveChanges();

            var registrationService = new RegistrationService(db);

            // Act
            var result = registrationService.All();

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

            var firstRegistration = new Registration { Id = registrationId, Flag = registrationFlagName };
            var secondRegistration = new Registration { Id = 2, Flag = "SecondFlag" };

            await db.AddRangeAsync(firstRegistration, secondRegistration);
            await db.SaveChangesAsync();

            var registrationService = new RegistrationService(db);

            // Act
            var result = await registrationService.GetCountAsync();

            // Assert
            var contentResult = Assert.IsType<System.Int32>(result);

            Assert.Equal(2, contentResult);
        }
    }
}
