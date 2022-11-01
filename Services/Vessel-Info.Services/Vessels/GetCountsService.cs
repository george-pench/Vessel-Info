namespace Vessel_Info.Services.Vessels
{
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Home;
    
    public class GetCountsService : IGetCountsService
    {
        private readonly IVesselService vessels;
        private readonly IRegistrationService registrations;
        private readonly IOwnerService owners;
        private readonly ITypeService types;
        private readonly IClassificationSocietyService classificationSocieties;
        private readonly IOperatorService operators;

        public GetCountsService(
            IVesselService vessels,
            IRegistrationService registrations,
            IOwnerService owners,
            ITypeService types,
            IClassificationSocietyService classificationSocieties,
            IOperatorService operators)
        {
            this.vessels = vessels;
            this.registrations = registrations;
            this.owners = owners;
            this.types = types;
            this.classificationSocieties = classificationSocieties;
            this.operators = operators;
        }

        public async Task<GetAllCountsServiceModel> GetAllCountsAsync() => new GetAllCountsServiceModel
        {
            VesselsCount = await this.vessels.GetCountAsync(),
            RegistrationsCount = await this.registrations.GetCountAsync(),
            OwnersCount = await this.owners.GetCountAsync(),
            TypesCount = await this.types.GetCountAsync(),
            ClassSocietiesCount = await this.classificationSocieties.GetCountAsync(),
            OperatorsCount = await this.operators.GetCountAsync()
        };
    }
}
