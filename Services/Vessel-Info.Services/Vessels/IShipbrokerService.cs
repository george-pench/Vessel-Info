namespace Vessel_Info.Services.Vessels
{
    using System.Threading.Tasks;
    
    public interface IShipbrokerService
    {
        Task<int> GetOrCreateShipbrokerAsync(string agencyName);
    }
}
