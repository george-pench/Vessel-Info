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

        public VesselService(
            VesselInfoDbContext dbContext, 
            IClassificationSocietyService classificationSociety,
            IRegistrationService registration)
        {
            this.dbContext = dbContext;
            this.classificationSociety = classificationSociety;
            this.registration = registration;
        }

        public IQueryable<VesselAllServiceModel> All() => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .To<VesselAllServiceModel>();

        public async Task<string> Create(VesselCreateServiceModel model)
        {
            var vessel = model.Vessel.To<Vessel>();

            var classificationSocietyFullName = model.ClassificationSociety.FullName;
            var classificationSocietyId = await this.classificationSociety.FindClassificationSocietyIdByName(classificationSocietyFullName);

            if (classificationSocietyId == 0)
            {
                classificationSocietyId = await this.classificationSociety.Create(model.ClassificationSociety);
            }

            vessel.ClassificationSocietyId = classificationSocietyId;

            var registrationName = model.Registration.Flag;
            var registrationId = await this.registration.FindRegistrationIdByName(registrationName);

            if (registrationId == 0)
            {
                registrationId = await this.registration.Create(model.Registration);
            }

            vessel.RegistrationId = registrationId;

            vessel.OwnerId = 1;
            vessel.TypeId = 1;

            await this.dbContext.Vessels.AddAsync(vessel);
            await this.dbContext.SaveChangesAsync();

            return vessel.Id;
        }

        public bool Edit(string id, VesselEditServiceModel model)
        {
            var edit = this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .FirstOrDefault();

            if (edit == null)
            {
                return false;
            }

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
            //edit.ExName = model.ExName;
            //edit.RegistrationId = model.Registration;
            //edit.TypeId = model.Type;
            //edit.ClassificationSocietyId = model.ClassificationSociety;
            //edit.OwnerId = model.Owner;

            int result = this.dbContext.SaveChanges();

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

        public bool Delete(string id)
        {
            var delete = this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .FirstOrDefault();

            this.dbContext.Vessels.Remove(delete);
            int result = this.dbContext.SaveChanges();

            return result > 0;
        }

        public async Task<VesselAllServiceModel> GetById(string id) => await this.dbContext
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
    }
}
