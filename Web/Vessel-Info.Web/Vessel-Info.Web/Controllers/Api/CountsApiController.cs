namespace Vessel_Info.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Home;
    using Vessel_Info.Services.Vessels;

    [ApiController]
    [Route("api/[controller]")]
    public class CountsApiController : ControllerBase
    {
        private readonly IGetCountsService counts;

        public CountsApiController(IGetCountsService counts) => this.counts = counts;

        [HttpGet]
        public async Task<GetAllCountsServiceModel> Get() => await this.counts.GetAllCountsAsync();
    }
}
