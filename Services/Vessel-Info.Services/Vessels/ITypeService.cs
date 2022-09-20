namespace Vessel_Info.Services.Vessels
{
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface ITypeService
    {
        Task<int> Create(VesselTypeServiceModel model);

        Task<int> FindTypeIdByName(string vesselType);
    }
}
