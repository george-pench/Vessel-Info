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

        public async Task<OwnerAllServiceModel> GetById(int? id) => await this.dbContext
                .Owners
                .Where(o => o.Id == id)
                .To<OwnerAllServiceModel>()
                .FirstOrDefaultAsync();

        public IQueryable<OwnerAllServiceModel> GetAllBySearchTerm(string searchTerm) => this.dbContext
               .Owners
               .Where(v => v.Name.StartsWith(searchTerm))
               .OrderBy(v => v.Name)
               .ThenBy(v => v.Id)
               .To<OwnerAllServiceModel>();

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

        public async Task<OwnerDetailsServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .Owners
                .Where(o => o.Id == id)
                .To<OwnerDetailsServiceModel>()
                .FirstOrDefaultAsync();
            
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<bool> EditAsync(int? id, OwnerEditServiceModel model)
        {
            var edit = await this.dbContext.Owners.FindAsync(id);

            if (edit == null)
            {
                return false;
            }

            edit.Name = model.Name;
            edit.Founded = model.Founded;
            edit.Website = model.Website;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> FindOwnerIdByName(string vesselOwner) => await this.dbContext
                .Owners
                .Where(o => o.Name == vesselOwner)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();

        public IQueryable<OwnerBaseServiceModel> AllPaging(int page, int pageSize = 10) => this.dbContext
                .Owners
                .OrderBy(o => o.Name)
                .ThenBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .To<OwnerBaseServiceModel>();

        public IQueryable<OwnerBaseServiceModel> All() => this.dbContext
                .Owners
                .OrderBy(o => o.Name)
                .To<OwnerBaseServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Owners.CountAsync();
    }
}
