namespace Vessel_Info.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
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
                .Select(v => new VesselAllViewModel 
                {
                    Id = v.Id,
                    Name = v.Name,
                    Imo = v.Imo,
                    Built = v.Built,
                    Dwt = v.Dwt,
                    Loa = v.Loa,
                    Cubic = v.Cubic,
                    Beam = v.Beam,
                    Draft = v.Draft,
                    Hull = v.Hull,
                    CallSign = v.CallSign
                })
                .ToList();

            return this.View(all);
        }

        // [HttpGet("Admin/Vessels/Details")]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = this.vessels.Details(id);

            var newDetails = new VesselDetailsViewModel 
            {
                Name = details.Name,
                Imo = details.Imo,
                Built = details.Built,
                Dwt = details.Dwt,
                Hull = details.Hull,
                CallSign = details.CallSign,
                ExName = details.ExName,
                Flag = details.Registration.Flag,
                RegistryPort = details.Registration.RegistryPort,
                VesselTypeName = details.VesselType.Name,
                ClassificationSocietyFullName = details.ClassSociety.FullName,
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
                    Dwt = model.Vessel.Dwt,
                    Loa = model.Vessel.Loa,
                    Cubic = model.Vessel.Cubic,
                    Beam = model.Vessel.Beam,
                    Draft = model.Vessel.Draft,
                    Hull = model.Vessel.Hull,
                    CallSign = model.Vessel.CallSign,                
                }
            };

            this.vessels.Create(vessel);

            return this.Redirect("/");
        }
    }
}
