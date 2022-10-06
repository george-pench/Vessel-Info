namespace Vessel_Info.Services.Vessels
{
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Home;
    
    public interface IGetCountsService
    {
        Task<GetAllCountsServiceModel> GetAllCounts();
    }
}
