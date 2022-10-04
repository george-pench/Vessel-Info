namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class ClassificationSocietyService : IClassificationSocietyService
    {
        private readonly VesselInfoDbContext dbContext;

        public ClassificationSocietyService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<int> GetOrCreateClassSocietyAsync(string classSocietyFullName)
        {
            var classSociety = await this.dbContext
                .ClassificationSocieties
                .FirstOrDefaultAsync(x => x.FullName == classSocietyFullName);

            if (classSociety != null)
            {
                return classSociety.Id;
            }

            classSociety = new ClassificationSociety
            {
                FullName = classSocietyFullName
            };

            await this.dbContext.ClassificationSocieties.AddAsync(classSociety);
            await this.dbContext.SaveChangesAsync();

            return classSociety.Id;
        }

        public async Task<VesselClassificationSocietyServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .ClassificationSocieties
                .Where(cs => cs.Id == id)
                .To<VesselClassificationSocietyServiceModel>()
                .FirstOrDefaultAsync();

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<int> FindClassificationSocietyIdByName(string vesselClass) => await this.dbContext
                .ClassificationSocieties
                .Where(cs => cs.FullName == vesselClass)
                .Select(cs => cs.Id)
                .FirstOrDefaultAsync();

        public IQueryable<VesselClassificationSocietyServiceModel> All() => dbContext
                .ClassificationSocieties
                .OrderBy(cs => cs.FullName)
                .To<VesselClassificationSocietyServiceModel>();
    }
}
