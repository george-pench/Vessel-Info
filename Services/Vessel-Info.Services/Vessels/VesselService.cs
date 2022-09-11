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
        public VesselService(VesselInfoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

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

        public IQueryable<VesselAllServiceModel> All() => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .To<VesselAllServiceModel>();

        public VesselDetailsServiceModel Details(string id) => this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselDetailsServiceModel>()
                .FirstOrDefault();

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
