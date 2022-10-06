namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class RegistrationService : IRegistrationService
    {
        private readonly VesselInfoDbContext dbContext;

        public RegistrationService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> GetOrCreateRegistrationAsync(string flagName, string registryPortName)
        {
            var registration = await this.dbContext
                .Registrations
                .FirstOrDefaultAsync(x => x.Flag == flagName);

            if (registration != null)
            {
                return registration.Id;
            }

            registration = new Registration
            {
                Flag = flagName,
                RegistryPort = registryPortName
            };

            await this.dbContext.Registrations.AddAsync(registration);
            await this.dbContext.SaveChangesAsync();

            return registration.Id;
        }

        public async Task<VesselRegistrationServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .Registrations
                .Where(r => r.Id == id)
                .To<VesselRegistrationServiceModel>()
                .FirstOrDefaultAsync();

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<int> FindRegistrationIdByName(string vesselRegistration) => await this.dbContext
                .Registrations
                .Where(r => r.Flag == vesselRegistration)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

        public IQueryable<VesselRegistrationServiceModel> All() => dbContext
                .Registrations
                .OrderBy(r => r.Flag)
                .To<VesselRegistrationServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Registrations.CountAsync();
    }
}
