namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;

    public class ShipbrokerService : IShipbrokerService
    {
        private readonly VesselInfoDbContext dbContext;

        public ShipbrokerService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> GetOrCreateShipbrokerAsync(string agencyName)
        {
            var shipbroker = await this.dbContext
                .Shipbrokers
                .FirstOrDefaultAsync(x => x.AgencyName == agencyName);

            if (shipbroker != null)
            {
                return shipbroker.Id;
            }

            shipbroker = new Shipbroker
            {
                AgencyName = agencyName
            };

            await this.dbContext.Shipbrokers.AddAsync(shipbroker);
            await this.dbContext.SaveChangesAsync();

            return shipbroker.Id;
        }
    }
}
