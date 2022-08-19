namespace Vessel_Info.Services.WebScraping
{  
    using Vessel_Info.Services.Models;
    
    public interface IQ88ScraperService
    {
        void PopulateDatabase();

        Q88ServiceModel GetVesselData(char id);
    }
}
