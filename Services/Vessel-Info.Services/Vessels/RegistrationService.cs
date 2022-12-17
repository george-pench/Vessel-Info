namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Registrations;

    public class RegistrationService : IRegistrationService
    {
        private readonly VesselInfoDbContext dbContext;

        public RegistrationService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<RegistrationBaseServiceModel> GetByIdAsync(int? id) => await this.dbContext
                .Registrations
                .Where(r => r.Id == id)
                .To<RegistrationBaseServiceModel>()
                .FirstOrDefaultAsync();

        public IQueryable<RegistrationBaseServiceModel> GetAllBySearchTerm(string searchTerm) => this.dbContext
                .Registrations
                .Where(v => v.Flag.StartsWith(searchTerm))
                .OrderBy(v => v.Flag)
                .ThenBy(v => v.Id)
                .To<RegistrationBaseServiceModel>();

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

        public async Task<RegistrationBaseServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .Registrations
                .Where(r => r.Id == id)
                .To<RegistrationBaseServiceModel>()
                .FirstOrDefaultAsync();

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<bool> EditAsync(int? id, RegistrationBaseServiceModel model)
        {
            var edit = await this.dbContext.Registrations.FindAsync(id);

            if (edit == null)
            {
                return false;
            }

            edit.Flag = model.Flag;
            edit.RegistryPort = model.RegistryPort;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> FindRegistrationIdByNameAsync(string vesselRegistration) => await this.dbContext
                .Registrations
                .Where(r => r.Flag == vesselRegistration)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

        public IQueryable<RegistrationBaseServiceModel> AllPaging(int page, int pageSize = 10) => this.dbContext
                .Registrations
                .OrderBy(r => r.Flag)
                .ThenBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .To<RegistrationBaseServiceModel>();

        public IQueryable<RegistrationBaseServiceModel> All() => dbContext
                .Registrations
                .OrderBy(r => r.Flag)
                .To<RegistrationBaseServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Registrations.CountAsync();
    }
}
