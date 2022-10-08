namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.ClassSocieties;

    public class ClassSocietiesController : Controller
    {
        private readonly IClassificationSocietyService classificationSocieties;

        public ClassSocietiesController(IClassificationSocietyService classificationSocieties)
        {
            this.classificationSocieties = classificationSocieties;
        }

        public IActionResult All()
        {
            var all = this.classificationSocieties
                .All()
                .To<ClassSocietyBaseViewModel>();

            return this.View(all);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = (await this.classificationSocieties
              .DetailsAsync(id))
              .To<ClassSocietyDetailsViewModel>();

            return this.View(details);
        }
    }
}
