namespace Vessel_Info.Web.ViewModels.ClassSocieties
{
    using System.Linq;
    
    public class ClassSocietyListingViewModel : PagingViewModel
    {
        public IQueryable<ClassSocietyBaseViewModel> ClassSocieties { get; set; }
    }
}
