namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models.Vessels;

    public class TypeService : ITypeService
    {
        private readonly VesselInfoDbContext dbContext;

        public TypeService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> Create(VesselTypeServiceModel model)
        {
            Type type = new Type
            {
                Name = model.Name
            };

            await this.dbContext.Types.AddAsync(type);
            await this.dbContext.SaveChangesAsync();

            return type.Id;
        }

        public async Task<int> FindTypeIdByName(string vesselType) => await this.dbContext
                .Types
                .Where(t => t.Name == vesselType)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();
    }
}
