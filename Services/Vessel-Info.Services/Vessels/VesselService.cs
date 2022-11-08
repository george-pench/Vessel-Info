namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;
    using Vessel_Info.Services.Models.Owners;
    using Vessel_Info.Services.Models.Registrations;
    using Vessel_Info.Services.Models.Types;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselService : IVesselService
    {
        private readonly VesselInfoDbContext dbContext;

        public VesselService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<string> CreateAsync(VesselFormServiceModel model)
        {
            var create = model.Vessel.To<Vessel>();

            create.RegistrationId = model.RegistrationId;
            create.TypeId = model.TypeId;
            create.OwnerId = model.OwnerId;
            create.ClassificationSocietyId = model.ClassificationSocietyId;
            create.ShipbrokerId = 1;

            await this.dbContext.Vessels.AddAsync(create);
            await this.dbContext.SaveChangesAsync();

            return create.Id;
        }

        public async Task<bool> EditAsync(string id, VesselFormServiceModel model)
        {
            var edit = await this.dbContext.Vessels.FindAsync(id);

            if (edit == null)
            {
                return false;
            }

            edit.Name = model.Vessel.Name;
            edit.Imo = model.Vessel.Imo;
            edit.Loa = model.Vessel.Loa;
            edit.HullType = model.Vessel.HullType;
            edit.Draft = model.Vessel.Draft;
            edit.CallSign = model.Vessel.CallSign;
            edit.Cubic = model.Vessel.Cubic;
            edit.Built = model.Vessel.Built;
            edit.Beam = model.Vessel.Beam;
            edit.SummerDwt = model.Vessel.SummerDwt;
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

        public IQueryable<VesselAllServiceModel> AllPaging(int page = 1, int pageSize = 10) => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .ThenBy(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .To<VesselAllServiceModel>();

        public IQueryable<VesselAllServiceModel> All() => this.dbContext
               .Vessels
               .OrderBy(v => v.Name)
               .ThenBy(v => v.Id)
               .To<VesselAllServiceModel>();

        public async Task<VesselAllServiceModel> GetByIdAsync(string id) => await this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselAllServiceModel>()
                .FirstOrDefaultAsync();

        public IQueryable<VesselAllServiceModel> GetAllBySearchTerm(string searchTerm) => this.dbContext
                .Vessels
                .Where(v => v.Name.StartsWith(searchTerm))
                .OrderBy(v => v.Name)
                .ThenBy(v => v.Id)
                .To<VesselAllServiceModel>();

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

        public IQueryable<VesselByTypeServiceModel> GetAllVesselByType() => this.dbContext
                .Vessels
                .Select(v => new VesselByTypeServiceModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    Imo = v.Imo,
                    SummerDwt = v.SummerDwt,
                    Built = v.Built,
                    VesselType = new TypeBaseServiceModel 
                    { 
                        Id = v.Type.Id, 
                        Name = v.Type.Name 
                    }
                });

        public IQueryable<VesselByRegistrationServiceModel> GetAllVesselByRegistration() => this.dbContext
                .Vessels
                .Select(v => new VesselByRegistrationServiceModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    Imo = v.Imo,
                    SummerDwt = v.SummerDwt,
                    Built = v.Built,
                    VesselRegistration = new RegistrationBaseServiceModel
                    {
                        Id = v.Registration.Id,
                        Flag = v.Registration.Flag,
                        RegistryPort = v.Registration.RegistryPort
                    }
                });

        public IQueryable<VesselByOwnerServiceModel> GetAllVesselByOwner() => this.dbContext
                .Vessels
                .Select(v => new VesselByOwnerServiceModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    Imo = v.Imo,
                    SummerDwt = v.SummerDwt,
                    Built = v.Built,
                    VesselOwner = new OwnerBaseServiceModel
                    {
                        Id = v.Owner.Id,
                        Name = v.Owner.Name
                    }
                });

        public IQueryable<VesselByClassSocietyServiceModel> GetAllVesselByClassSociety() => this.dbContext
                .Vessels
                .Select(v => new VesselByClassSocietyServiceModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    Imo = v.Imo,
                    SummerDwt = v.SummerDwt,
                    Built = v.Built,
                    VesselClassSociety = new ClassSocietyBaseServiceModel
                    {
                        Id = v.ClassificationSociety.Id,
                        FullName = v.ClassificationSociety.FullName
                    }
                });
    }
}
