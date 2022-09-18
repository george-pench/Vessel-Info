namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models.Vessels;

    public class RegistrationService : IRegistrationService
    {
        private readonly VesselInfoDbContext dbContext;

        public RegistrationService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> Create(VesselRegistrationServiceModel model)
        {
            Registration registration = new Registration
            {
                Flag = model.Flag,
                RegistryPort = model.RegistryPort
            };

            await this.dbContext.Registrations.AddAsync(registration);
            await this.dbContext.SaveChangesAsync();

            return registration.Id;
        }

        public async Task<int> FindRegistrationIdByName(string vesselRegistration) => await this.dbContext
                .Registrations
                .Where(r => r.Flag == vesselRegistration)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
    }
}
