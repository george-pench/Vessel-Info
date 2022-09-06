namespace Vessel_Info.Services.Vessels
{
    using System.Collections.Generic;
    using System.Linq;
    using Vessel_Info.Data;
    using Vessel_Info.Services.Vessels.Models;

    public class VesselService : IVesselService
    {
        private readonly VesselInfoDbContext dbContext;

        public VesselService(VesselInfoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public IEnumerable<VesselAllServiceModel> All()
        {
            return this.dbContext
                .Vessels
                .OrderBy(v => v.Name)
                .Select(v => new VesselAllServiceModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    Imo = v.Imo,
                    Built = v.Built,
                    Dwt = v.SummerDwt,
                    Loa = v.Loa,
                    Cubic = v.Cubic,
                    Beam = v.Beam,
                    Draft = v.Draft,
                    Hull = v.HullType,
                    CallSign = v.CallSign
                })
                .ToList();
        }

        public VesselDetailsServiceModel Details(string id)
        {
            var vessel = this.dbContext
                .Vessels
                .Where(v => v.Id == id)
                .Select(v => new VesselDetailsServiceModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    ExName = string.Empty,
                    Registration = new VesselRegistrationServiceModel
                    {
                        Flag = v.Registration.Flag,
                        RegistryPort = v.Registration.RegistryPort
                    },
                    VesselType = new VesselTypeServiceModel
                    {
                        Name = v.Type.Name
                    },
                    ClassSociety = new VesselClassificationSocietyServiceModel
                    {
                        FullName = v.ClassificationSociety.FullName
                    },
                    Imo = v.Imo,
                    CallSign = v.CallSign,
                    Dwt = v.SummerDwt,
                    Built = v.Built,
                    Hull = v.HullType,
                    Owner = new VesselOwnerServiceModel
                    {
                        Name = v.Owner.Name
                    }
                })
                .FirstOrDefault();                
            
            return vessel;
        }
    }
}
