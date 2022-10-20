namespace Vessel_Info.Web.ViewModels.Home
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Home;

    public class IndexViewModel : IMapFrom<GetAllCountsServiceModel>
    {
        public int VesselsCount { get; set; }

        public int RegistrationsCount { get; set; }

        public int OwnersCount { get; set; }

        public int TypesCount { get; set; }

        public int ClassSocietiesCount { get; set; }

        public int OperatorsCount { get; set; }
    }
}
