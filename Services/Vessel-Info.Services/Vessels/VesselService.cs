namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselService : IVesselService
    {
        private readonly VesselInfoDbContext dbContext;
        public VesselService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public bool Create(VesselCreateServiceModel model)
        {
            var vessel = model.Vessel.To<Vessel>();

            vessel.RegistrationId = model.RegistrationId;
            vessel.TypeId = model.TypeId;
            vessel.ClassificationSocietyId = model.ClassificationSocietyId;
            vessel.OwnerId = model.OwnerId;

            this.dbContext.Vessels.Add(vessel);
            int result = this.dbContext.SaveChanges();

            return result > 0;
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

        public bool Edit(string id, VesselEditServiceModel model)
        {
            var edit = this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .FirstOrDefault();

            var edit2 = this.dbContext
                .Vessels
                .Find(id);

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

        public IQueryable<VesselAllServiceModel> All() => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .To<VesselAllServiceModel>();

        public VesselDetailsServiceModel Details(string id)
        {
            var details = this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselDetailsServiceModel>()
                .FirstOrDefault();

            details.HullType = HullTypeFullName(details.HullType); 

            return details;
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

        public VesselAllServiceModel GetById(string id) => this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselAllServiceModel>()
                .FirstOrDefault();
    }
}
