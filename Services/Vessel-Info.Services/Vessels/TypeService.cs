﻿namespace Vessel_Info.Services.Vessels
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

        public Task<TypeBaseServiceModel> DetailsAsync(int? id)
        {
            var details = this.dbContext
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

        public async Task<int> FindTypeIdByName(string vesselType) => await this.dbContext
                .Types
                .Where(t => t.Name == vesselType)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

        public IQueryable<TypeBaseServiceModel> All() => this.dbContext
                .Types
                .OrderBy(t => t.Name)
                .To<TypeBaseServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Types.CountAsync();

        public async Task<int> GetTypeMaxCountByFrequencyAsync()
        {
            var max = await this.dbContext
                .Types
                .GroupBy(x => new { x.Id, x.Name })
                .OrderByDescending(x => x.Count())                
                .Select(x => new
                {
                    IdMaxCount = x.Key.Id,
                    Name = x.Key.Name
                })
                .FirstAsync();

            return max.IdMaxCount;
        }
    }
}
