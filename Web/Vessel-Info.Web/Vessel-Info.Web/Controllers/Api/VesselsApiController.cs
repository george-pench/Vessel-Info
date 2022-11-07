namespace Vessel_Info.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Services.Models.Vessels.Api;
    using Vessel_Info.Services.Vessels;

    [ApiController]
    [Route("api/[controller]")]
    public class VesselsApiController : ControllerBase
    {
        // TODO: implement POST/PUT and add data validation
        private readonly IVesselService vessels;

        public VesselsApiController(IVesselService vessels) => this.vessels = vessels;

        // GET: api/<VesselsApiController>
        [HttpGet]
        public IEnumerable<VesselAllServiceModel> Get()
        {
            return this.vessels.All();
        }

        // GET api/<VesselsApiController>/007d004e-8bf5-4015-81ec-e19b29ff0c4d
        [HttpGet("{id}")]
        public async Task<VesselAllServiceModel> Get(string id)
        {
            return await this.vessels.GetByIdAsync(id);
        }

        // POST api/<VesselsApiController>
        [HttpPost]
        public IActionResult Post([FromBody]VesselRequestModel model)
        {
            
        }

        // PUT api/<VesselsApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VesselsApiController>/00048ecf-69b7-474b-b4e6-e249d01cf1f3
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }

            await this.vessels.DeleteAsync(id);

            return this.Ok();
        }
    }
}
