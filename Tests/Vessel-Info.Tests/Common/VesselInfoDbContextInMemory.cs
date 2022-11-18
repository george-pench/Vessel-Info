namespace Vessel_Info.Tests.Common
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using Vessel_Info.Data;

    public static class VesselInfoDbContextInMemory
    {
        public static VesselInfoDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<VesselInfoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new VesselInfoDbContext(dbOptions);
        }
    }
}
