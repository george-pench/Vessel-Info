namespace Vessel_Info.Services.WebScraping
{  
    public interface IQ88ScraperService
    {
        void PopulateDatabase();

        void GetVesselData(char id);
    }
}
