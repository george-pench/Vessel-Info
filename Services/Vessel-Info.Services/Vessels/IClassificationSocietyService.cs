namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.ClassSocieties;

    public interface IClassificationSocietyService
    {
        Task<ClassSocietyAllServiceModel> GetByIdAsync(int? id);

        Task<int> GetOrCreateClassSocietyAsync(string classSocietyFullName);

        Task<ClassSocietyDetailsServiceModel> DetailsAsync(int? id);

        Task<bool> EditAsync(int? id, ClassSocietyEditServiceModel model);

        Task<int> FindClassSocietyIdByNameAsync(string vesselClass);

        IQueryable<ClassSocietyBaseServiceModel> All();

        Task<int> GetCountAsync();
    }
}
