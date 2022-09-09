namespace Vessel_Info.Services.Vessels
{
    using System;
    using System.Linq;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselService : IVesselService
    {
        // TODO: use AutoMapper 
        private readonly VesselInfoDbContext dbContext;

        public VesselService(VesselInfoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool Create(VesselCreateServiceModel model)
        {
            var vessel = new Vessel 
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Vessel.Name,
                Imo = model.Vessel.Imo,
                Built = model.Vessel.Built,
                SummerDwt = model.Vessel.SummerDwt,
                Loa = model.Vessel.Loa,
                Cubic = model.Vessel.Cubic,
                Beam = model.Vessel.Beam,
                Draft = model.Vessel.Draft,
                HullType = model.Vessel.Hull,
                CallSign = model.Vessel.CallSign,
                TypeId = 1,
                OwnerId = 1,
                RegistrationId = 1,
                ClassificationSocietyId = 1,
            };

            this.dbContext.Vessels.Add(vessel);
            int result = this.dbContext.SaveChanges();

            return result > 0;
        }

        public IQueryable<VesselAllServiceModel> All() => this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .To<VesselAllServiceModel>();

        public VesselDetailsServiceModel Details(string id)
        {
            //this.dbContext
            //    .Vessels
            //    .Where(v => v.Id == id)
            //    .Select(v => new VesselDetailsServiceModel
            //    {
            //        Id = v.Id,
            //        Name = v.Name,
            //        ExName = string.Empty,
            //        Registration = new VesselRegistrationServiceModel
            //        {
            //            Flag = v.Registration.Flag,
            //            RegistryPort = v.Registration.RegistryPort
            //        },
            //        VesselType = new VesselTypeServiceModel
            //        {
            //            Name = v.Type.Name
            //        },
            //        ClassSociety = new VesselClassificationSocietyServiceModel
            //        {
            //            FullName = v.ClassificationSociety.FullName
            //        },
            //        Imo = v.Imo,
            //        CallSign = v.CallSign,
            //        Dwt = v.SummerDwt,
            //        Built = v.Built,
            //        Hull = HullTypeFullName(v.HullType),
            //        Owner = new VesselOwnerServiceModel
            //        {
            //            Name = v.Owner.Name
            //        }
            //    })
            //    .FirstOrDefault();

            var details = this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .To<VesselDetailsServiceModel>()
                .FirstOrDefault();

            return details;
        }

        private static string HullTypeFullName(string hullType) 
            => hullType switch
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
