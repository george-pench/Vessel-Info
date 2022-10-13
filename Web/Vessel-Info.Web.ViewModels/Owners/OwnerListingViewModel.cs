namespace Vessel_Info.Web.ViewModels.Owners
{
    using System.Linq;
    
    public class OwnerListingViewModel : PagingViewModel
    {
        public IQueryable<OwnerDetailsViewModel> Owners { get; set; }
    }
}
