namespace Vessel_Info.Web.ViewModels.Owners
{
    using System.Linq;
    
    public class OwnerListingViewModel : PagingViewModel
    {
        public IQueryable<OwnerBaseViewModel> Owners { get; set; }
    }
}
