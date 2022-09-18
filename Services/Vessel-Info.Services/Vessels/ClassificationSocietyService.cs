namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models.Vessels;

    public class ClassificationSocietyService : IClassificationSocietyService
    {
        private readonly VesselInfoDbContext dbContext;

        public ClassificationSocietyService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> Create(VesselClassificationSocietyServiceModel model)
        {
            ClassificationSociety classificationSociety = new ClassificationSociety
            {
                FullName = model.FullName
            };

            await this.dbContext.ClassificationSocieties.AddAsync(classificationSociety);
            await this.dbContext.SaveChangesAsync();

            return classificationSociety.Id;
        }

        public async Task<int> FindClassificationSocietyIdByName(string vesselClass) => await this.dbContext
                .ClassificationSocieties
                .Where(cs => cs.FullName == vesselClass)
                .Select(cs => cs.Id)
                .FirstOrDefaultAsync();
    }
}
