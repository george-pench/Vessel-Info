namespace Vessel_Info.Web.ViewModels.Types
{
    using System.Linq;
    
    public class TypeListingViewModel : PagingViewModel
    {
        public IQueryable<TypeBaseViewModel> Types { get; set; }
    }
}
