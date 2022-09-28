namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class OwnerService : IOwnerService
    {
        private readonly VesselInfoDbContext dbContext;

        public OwnerService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> Create(VesselOwnerServiceModel model)
        {
            Owner owner = new Owner
            {
                Name = model.Name
            };

            await this.dbContext.Owners.AddAsync(owner);
            await this.dbContext.SaveChangesAsync();

            return owner.Id;
        }

        public async Task<int> FindOwnerIdByName(string vesselOwner) => await this.dbContext
                .Owners
                .Where(o => o.Name == vesselOwner)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();

        public IQueryable<VesselOwnerServiceModel> All() => dbContext
                .Owners
                .OrderBy(o => o.Name)
                .To<VesselOwnerServiceModel>();
    }
}
