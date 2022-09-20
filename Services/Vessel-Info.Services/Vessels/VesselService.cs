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

        public IQueryable<VesselAllServiceModel> All() => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .To<VesselAllServiceModel>();

        public async Task<string> Create(VesselCreateServiceModel model)
        {
            var vessel = model.Vessel.To<Vessel>();

            int classificationSocietyId = await this.GetClassificationSocietyId(model);
            vessel.ClassificationSocietyId = classificationSocietyId;

            int registrationId = await this.GetRegistrationId(model);
            vessel.RegistrationId = registrationId;

            int ownerId = await this.GetOwnerId(model);
            vessel.OwnerId = ownerId;

            int typeId = await this.GetTypeId(model);
            vessel.TypeId = typeId;

            await this.dbContext.Vessels.AddAsync(vessel);
            await this.dbContext.SaveChangesAsync();

            return vessel.Id;
        }

        public async Task<bool> Edit(string id, VesselEditServiceModel model)
        {
            var edit = await this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (edit == null)
            {
                return false;
            }

            //var e = model.To<Vessel>();

            edit.Name = model.Name;
            edit.Imo = model.Imo;
            edit.Built = model.Built;
            edit.SummerDwt = model.SummerDwt;
            edit.Loa = model.Loa;
            edit.Cubic = model.Cubic;
            edit.Beam = model.Beam;
            edit.Draft = model.Draft;
            edit.Name = model.Name;
            edit.Imo = model.Imo;
            edit.HullType = model.HullType;
            edit.CallSign = model.CallSign;
            edit.RegistrationId = model.Registration.Id;
            edit.TypeId = model.Type.Id;
            edit.ClassificationSocietyId = model.ClassificationSociety.Id;
            edit.OwnerId = model.Owner.Id;

            int result = await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<VesselDetailsServiceModel> Details(string id)
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

        public void Delete(string id)
        {
            var delete = this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .FirstOrDefault();

            if (delete == null)
            {
                throw new ArgumentNullException(nameof(delete));
            }

            this.dbContext.Vessels.Remove(delete);
            this.dbContext.SaveChanges();            
        }

        public async Task<VesselAllServiceModel> GetById(string id) => await this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselAllServiceModel>()
                .FirstOrDefaultAsync();

        private async Task<int> GetTypeId(VesselCreateServiceModel model)
        {
            var typeName = model.Type.Name;
            var typeId = await this.type.FindTypeIdByName(typeName);

            if (typeId == 0)
            {
                typeId = await this.type.Create(model.Type);
            }

            return typeId;
        }

        private async Task<int> GetOwnerId(VesselCreateServiceModel model)
        {
            var ownerName = model.Owner.Name;
            var ownerId = await this.owner.FindOwnerIdByName(ownerName);

            if (ownerId == 0)
            {
                ownerId = await this.owner.Create(model.Owner);
            }

            return ownerId;
        }

        private async Task<int> GetRegistrationId(VesselCreateServiceModel model)
        {
            var registrationName = model.Registration.Flag;
            var registrationId = await this.registration.FindRegistrationIdByName(registrationName);

            if (registrationId == 0)
            {
                registrationId = await this.registration.Create(model.Registration);
            }

            return registrationId;
        }

        private async Task<int> GetClassificationSocietyId(VesselCreateServiceModel model)
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
