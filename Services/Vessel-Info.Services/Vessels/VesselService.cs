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

        private readonly IClassificationSocietyService classificationSociety;
        private readonly IRegistrationService registration;
        private readonly IOwnerService owner;
        private readonly ITypeService type;

        public VesselService(
            VesselInfoDbContext dbContext, 
            IClassificationSocietyService classificationSociety,
            IRegistrationService registration,
            IOwnerService owner,
            ITypeService type)
        {
            this.dbContext = dbContext;

            this.classificationSociety = classificationSociety;
            this.registration = registration;
            this.owner = owner;
            this.type = type;
        }

        public async Task<string> CreateAsync(VesselCreateServiceModel model)
        {
            var create = model.Vessel.To<Vessel>();

            int classificationSocietyId = await this.GetOrCreateClassificationSocietyAsync(model);            
            create.ClassificationSocietyId = classificationSocietyId;

            int registrationId = await this.GetOrCreateRegistrationAsync(model);
            create.RegistrationId = registrationId;

            int ownerId = await this.GetOrCreateOwnerAsync(model);
            create.OwnerId = ownerId;

            int typeId = await this.GetOrCreateTypeAsync(model);
            create.TypeId = typeId;

            await this.dbContext.Vessels.AddAsync(create);
            await this.dbContext.SaveChangesAsync();

            return create.Id;
        }

        public async Task<bool> EditAsync(string id, VesselEditServiceModel model)
        {
            var edit = model.Vessel.To<Vessel>();
            //TODO: bind ids

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
        public IQueryable<VesselAllServiceModel> All() => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .To<VesselAllServiceModel>();

        public async Task<VesselAllServiceModel> GetByIdAsync(string id) => await this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselAllServiceModel>()
                .FirstOrDefaultAsync();

        // TODO: move GetOrCreate methods to corresponding services
        private async Task<int> GetOrCreateTypeAsync(VesselCreateServiceModel model)
        {
            var typeName = model.Type.Name;
            var typeId = await this.type.FindTypeIdByName(typeName);

            if (typeId == 0)
            {
                typeId = await this.type.Create(model.Type);
            }

            return typeId;
        }

        private async Task<int> GetOrCreateOwnerAsync(VesselCreateServiceModel model)
        {
            var ownerName = model.Owner.Name;
            var ownerId = await this.owner.FindOwnerIdByName(ownerName);

            if (ownerId == 0)
            {
                ownerId = await this.owner.Create(model.Owner);
            }

            return ownerId;
        }

        private async Task<int> GetOrCreateRegistrationAsync(VesselCreateServiceModel model)
        {
            var registrationName = model.Registration.Flag;
            var registrationId = await this.registration.FindRegistrationIdByName(registrationName);

            if (registrationId == 0)
            {
                registrationId = await this.registration.Create(model.Registration);
            }

            return registrationId;
        }

        private async Task<int> GetOrCreateClassificationSocietyAsync(VesselCreateServiceModel model)
        {
            var classificationSocietyFullName = model.ClassificationSociety.FullName;
            var classificationSocietyId = await this.classificationSociety.FindClassificationSocietyIdByName(classificationSocietyFullName);

            if (classificationSocietyId == 0)
            {
                classificationSocietyId = await this.classificationSociety.Create(model.ClassificationSociety);
            }

            return classificationSocietyId;
        }

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
    }
}
