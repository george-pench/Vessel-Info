namespace Vessel_Info.Web.ViewModels.Vessels
{
    using System.Linq;

    public class VesselListingViewModel : PagingViewModel
    {
        public IQueryable<VesselAllViewModel> Vessels { get; set; }
    }
}
