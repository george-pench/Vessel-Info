namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Operators;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Operators;

    using static Vessel_Info.Web.Constants.WebConstants;

    public class OperatorsController : Controller
    {
        private readonly IOperatorService operators;

        public OperatorsController(IOperatorService operators) => this.operators = operators;

        public IActionResult Search(string searchTerm) => this.View(new OperatorListingViewModel
        {
            Operators = this.operators.GetAllBySearchTerm(searchTerm).To<OperatorBaseViewModel>()
        });

        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            return this.View(new OperatorListingViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EntityCount = await this.operators.GetCountAsync(),
                Operators = this.operators.AllPaging(id, ItemsPerPage).To<OperatorDetailsViewModel>()
            });
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = (await this.operators
                .DetailsAsync(id))
                .To<OperatorDetailsViewModel>();

            return this.View(details);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var edit = (await this.operators
                .GetByIdAsync(id))
                .To<OperatorEditInputModel>();

            return this.View(edit);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, OperatorEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(model);
            }

            var edit = ObjectMappingExtensions.To<OperatorEditServiceModel>(model);

            await this.operators.EditAsync(id, edit);

            return this.RedirectToAction(nameof(OperatorsController.Details), new { id = id });
        }
    }
}
