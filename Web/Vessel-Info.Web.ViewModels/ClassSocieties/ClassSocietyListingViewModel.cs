namespace Vessel_Info.Web.ViewModels.ClassSocieties
{
    using System.Linq;
    
    public class ClassSocietyListingViewModel : PagingViewModel
    {
        public IQueryable<ClassSocietyDetailsViewModel> ClassSocieties { get; set; }
    }
}
