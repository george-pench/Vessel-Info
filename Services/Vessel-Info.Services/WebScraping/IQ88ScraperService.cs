namespace Vessel_Info.Services.WebScraping
{
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models;
    
    public interface IQ88ScraperService
    {
        Task PopulateDatabase();

        Q88ServiceModel GetVesselData(char id);
    }
}
