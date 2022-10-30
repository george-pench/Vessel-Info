namespace Vessel_Info.Web.ViewModels.Registrations
{
    using System.Linq;
    
    public class RegistrationListingViewModel : PagingViewModel
    {
        public IQueryable<RegistrationBaseViewModel> Registrations { get; set; }
    }
}
