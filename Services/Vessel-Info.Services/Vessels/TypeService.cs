namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class TypeService : ITypeService
    {
        private readonly VesselInfoDbContext dbContext;

        public TypeService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> GetOrCreateTypeAsync(string typeName)
        {
            var type = await this.dbContext
                .Types
                .FirstOrDefaultAsync(x => x.Name == typeName);

            if (type != null)
            {
                return type.Id;
            }

            type = new Data.Models.Type
            {
                Name = typeName
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

        public IQueryable<VesselTypeServiceModel> All() => dbContext
                .Types
                .OrderBy(t => t.Name)
                .To<VesselTypeServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Types.CountAsync();
    }
}
