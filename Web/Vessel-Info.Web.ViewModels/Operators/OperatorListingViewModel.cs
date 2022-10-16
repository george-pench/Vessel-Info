namespace Vessel_Info.Web.ViewModels.Operators
{
    using System.Linq;
    
    public class OperatorListingViewModel : PagingViewModel
    {
         public IQueryable<OperatorDetailsViewModel> Operators { get; set; }
    }
}
