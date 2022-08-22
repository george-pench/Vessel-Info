namespace Vessel_Info.Services.WebScraping
{ 
    using System.Threading.Tasks;
    
    public interface IQ88ScraperService
    {
        Task ImportVesselDataAsync(char fromId = 'A', char toId = 'Z');
    }
}
