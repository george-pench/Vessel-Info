namespace Vessel_Info.Tests.Services
{
    using Data;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Tests.Common;
    using Xunit;

    public class ClassificationSocietyServiceTest
    {
        // TODO: AAA patern

        public ClassificationSocietyServiceTest()
        {
            MapperInitializer.Initialize();
        }

        [Fact]
        public void GetAllBySearchTermShouldReturnCorrectResultWithWhereAndOrder()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<VesselInfoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new VesselInfoDbContext(dbOptions);

            var firstClass = new ClassificationSociety { Id = 1, FullName = "First" };
            var secondClass = new ClassificationSociety { Id = 2, FullName = "Second" };
            var thirdClass = new ClassificationSociety { Id = 3, FullName = "Third" };

            db.AddRange(firstClass, secondClass, thirdClass);
            db.SaveChanges();

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
    }
}
