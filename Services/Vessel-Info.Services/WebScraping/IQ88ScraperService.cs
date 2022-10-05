namespace Vessel_Info.Services.WebScraping
{ 
    using System.Threading.Tasks;

    using static Constants.ServicesConstants;

    public interface IQ88ScraperService
    {
        Task ImportVesselDataAsync(char fromId = StartLetter, char toId = EndLetter);
    }
}
