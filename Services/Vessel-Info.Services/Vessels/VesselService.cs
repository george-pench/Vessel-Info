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

    public class VesselService : IVesselService
    {
        private readonly VesselInfoDbContext dbContext;

        public VesselService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<string> CreateAsync(VesselCreateServiceModel model)
        {
            var create = model.Vessel.To<Vessel>();
            
            // TODO: try binding createService' ids to Vessel' ids
            create.RegistrationId = model.RegistrationId;
            create.TypeId = model.TypeId;
            create.OwnerId = model.OwnerId;
            create.ClassificationSocietyId = model.ClassificationSocietyId;

            await this.dbContext.Vessels.AddAsync(create);
            await this.dbContext.SaveChangesAsync();

            return create.Id;
        }

        public async Task<bool> EditAsync(string id, VesselEditServiceModel model)
        {
            var edit = await this.dbContext.Vessels.FindAsync(id);

            if (edit == null)
            {
                return false;
            }

            edit.RegistrationId = model.RegistrationId;
            edit.TypeId = model.TypeId;
            edit.ClassificationSocietyId = model.ClassificationSocietyId;
            edit.OwnerId = model.OwnerId;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<VesselDetailsServiceModel> DetailsAsync(string id)
        {
            var details = await this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselDetailsServiceModel>()
                .FirstOrDefaultAsync();

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            details.HullType = HullTypeFullName(details.HullType); 

            return details;
        }

        public async Task DeleteAsync(string id)
        {
            var delete = await this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (delete == null)
            {
                throw new ArgumentNullException(nameof(delete));
            }

            this.dbContext.Vessels.Remove(delete);
            await this.dbContext.SaveChangesAsync();            
        }

        public IQueryable<VesselAllServiceModel> All(int page = 1, int pageSize = 10) => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .ThenBy(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .To<VesselAllServiceModel>();

        public async Task<VesselAllServiceModel> GetByIdAsync(string id) => await this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselAllServiceModel>()
                .FirstOrDefaultAsync();

        private static string HullTypeFullName(string hullType) => hullType switch
        {
            "DB" => "Double Bottom",
            "DH" => "Double Hull",
            "DS" => "Double Side",
            "SB" => "Single Bottom",
            "SH" => "Single Hull",
            "SS" => "Single Side",
            _ => "Other",
        };

        public async Task<int> GetCountAsync() => await dbContext.Vessels.CountAsync();
    }
}
