namespace Vessel_Info.Web.ViewModels.Operators
{
    using System.Linq;
    
    public class OperatorListingViewModel : PagingViewModel
    {
         public IQueryable<OperatorBaseViewModel> Operators { get; set; }
    }
}
