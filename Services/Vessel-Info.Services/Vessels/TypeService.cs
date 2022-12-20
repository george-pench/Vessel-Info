namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Types;

    public class TypeService : ITypeService
    {
        private readonly VesselInfoDbContext dbContext;

        public TypeService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public IQueryable<TypeBaseServiceModel> GetAllBySearchTerm(string searchTerm) => this.dbContext
               .Types
               .Where(v => v.Name.StartsWith(searchTerm))
               .OrderBy(v => v.Name)
               .ThenBy(v => v.Id)
               .To<TypeBaseServiceModel>();

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

        public async Task<TypeBaseServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .Types
                .Where(t => t.Id == id)
                .To<TypeBaseServiceModel>()
                .FirstOrDefaultAsync();

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<int> FindTypeIdByNameAsync(string vesselType) => await this.dbContext
                .Types
                .Where(t => t.Name == vesselType)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

        public IQueryable<TypeBaseServiceModel> AllPaging(int page, int pageSize = 10) => this.dbContext
                .Types
                .OrderBy(t => t.Name)
                .ThenBy(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .To<TypeBaseServiceModel>();

        public IQueryable<TypeBaseServiceModel> All() => this.dbContext
                .Types
                .OrderBy(t => t.Name)
                .To<TypeBaseServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Types.CountAsync();
    }
}
