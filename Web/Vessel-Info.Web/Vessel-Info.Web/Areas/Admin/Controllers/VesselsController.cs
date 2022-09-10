namespace Vessel_Info.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Vessels;

    public class VesselsController : AdminController
    {
        private readonly IVesselService vessels;

        public VesselsController(IVesselService vessels)
        {
            this.vessels = vessels;
        }

        // [HttpGet("Admin/Vessels/All")]
        public IActionResult All()
        {
            var all = this.vessels
                .All()
                .To<VesselAllViewModel>();

            return this.View(all);
        }

        // [HttpGet("Admin/Vessels/Details")]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            VesselDetailsServiceModel details = this.vessels.Details(id);

            var newDetails = new VesselDetailsViewModel
            {
                Name = details.Name,
                Imo = details.Imo,
                Built = details.Built,
                SummerDwt = details.SummerDwt,
                Hull = details.Hull,
                CallSign = details.CallSign,
                ExName = details.ExName,
                Flag = details.Registration.Flag,
                RegistryPort = details.Registration.RegistryPort,
                TypeName = details.Type.Name,
                ClassificationSocietyFullName = details.ClassificationSociety.FullName,
                OwnerName = details.Owner.Name
            };           

            if (!this.ModelState.IsValid)
            {
                return this.View(newDetails);
            }

            return this.View(newDetails);
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(VesselCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var vessel = new VesselCreateServiceModel   
            {
                TypeId = 1,
                RegistrationId = 1,
                ClassificationSocietyId = 1,
                OwnerId = 1,
                Vessel = new VesselAllServiceModel
                {
                    Id = model.Vessel.Id,
                    Name = model.Vessel.Name,
                    Imo = model.Vessel.Imo,
                    Built = model.Vessel.Built,
                    SummerDwt = model.Vessel.SummerDwt,
                    Loa = model.Vessel.Loa,
                    Cubic = model.Vessel.Cubic,
                    Beam = model.Vessel.Beam,
                    Draft = model.Vessel.Draft,
                    Hull = model.Vessel.Hull,
                    CallSign = model.Vessel.CallSign,                
                }
            };
            
            this.vessels.Create(vessel);

            var newVessel = new VesselCreateInputModel 
            {
                TypeId = vessel.TypeId,
                RegistrationId = vessel.RegistrationId,
                ClassificationSocietyId = vessel.ClassificationSocietyId,
                OwnerId = vessel.OwnerId,
                Vessel = new VesselAllViewModel 
                {
                    Id = vessel.Vessel.Id,
                    Name = vessel.Vessel.Name,
                    Imo = vessel.Vessel.Imo,
                    Built = vessel.Vessel.Built,
                    SummerDwt = vessel.Vessel.SummerDwt,
                    Loa = vessel.Vessel.Loa,
                    Cubic = vessel.Vessel.Cubic,
                    Beam = vessel.Vessel.Beam,
                    Draft = vessel.Vessel.Draft,
                    Hull = vessel.Vessel.Hull,
                    CallSign = vessel.Vessel.CallSign,
                }
            };

            return this.Redirect("/");
        }
    }
}
