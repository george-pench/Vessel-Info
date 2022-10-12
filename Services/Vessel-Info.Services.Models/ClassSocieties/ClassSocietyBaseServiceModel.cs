namespace Vessel_Info.Services.Models.ClassSocieties
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class ClassSocietyBaseServiceModel : IMapFrom<ClassificationSociety>, IMapTo<ClassificationSociety>
    {
        public int Id { get; set; }

        public string FullName { get; set; }
    }
}
