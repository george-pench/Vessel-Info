namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Owners;

    public class OwnerService : IOwnerService
    {
        private readonly VesselInfoDbContext dbContext;

        public OwnerService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> GetOrCreateOwnerAsync(string ownerName)
        {
            var owner = await this.dbContext
                .Owners
                .FirstOrDefaultAsync(x => x.Name == ownerName);

            if (owner != null)
            {
                return owner.Id;
            }

            owner = new Owner
            {
                Name = ownerName
            };

            await this.dbContext.Owners.AddAsync(owner);
            await this.dbContext.SaveChangesAsync();

            return owner.Id;
        }

        public async Task<OwnerBaseServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .Owners
                .Where(o => o.Id == id)
                .To<OwnerBaseServiceModel>()
                .FirstOrDefaultAsync();
            
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<int> FindOwnerIdByName(string vesselOwner) => await this.dbContext
                .Owners
                .Where(o => o.Name == vesselOwner)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();

        public IQueryable<OwnerBaseServiceModel> All() => dbContext
                .Owners
                .OrderBy(o => o.Name)
                .To<OwnerBaseServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Owners.CountAsync();
    }
}
