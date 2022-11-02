namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;

    public class ClassificationSocietyService : IClassificationSocietyService
    {
        private readonly VesselInfoDbContext dbContext;

        public ClassificationSocietyService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<ClassSocietyAllServiceModel> GetByIdAsync(int? id) => await this.dbContext
            .ClassificationSocieties
            .Where(cs => cs.Id == id)
            .To<ClassSocietyAllServiceModel>()
            .FirstOrDefaultAsync();

        public IQueryable<ClassSocietyAllServiceModel> GetAllBySearchTerm(string searchTerm) => this.dbContext
               .ClassificationSocieties
               .Where(v => v.FullName.StartsWith(searchTerm))
               .OrderBy(v => v.FullName)
               .ThenBy(v => v.Id)
               .To<ClassSocietyAllServiceModel>();

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

        public async Task<ClassSocietyDetailsServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .ClassificationSocieties
                .Where(cs => cs.Id == id)
                .To<ClassSocietyDetailsServiceModel>()
                .FirstOrDefaultAsync();

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<bool> EditAsync(int? id, ClassSocietyEditServiceModel model)
        {
            var edit = await this.dbContext.ClassificationSocieties.FindAsync(id);

            if (edit == null)
            {
                return false;
            }

            edit.FullName = model.FullName;
            edit.Abbreviation = model.Abbreviation;
            edit.Founded = model.Founded;
            edit.Website = model.Website;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> FindClassSocietyIdByNameAsync(string vesselClass) => await this.dbContext
                .ClassificationSocieties
                .Where(cs => cs.FullName == vesselClass)
                .Select(cs => cs.Id)
                .FirstOrDefaultAsync();

        public IQueryable<ClassSocietyBaseServiceModel> AllPaging(int page, int pageSize = 10) => this.dbContext
               .ClassificationSocieties
               .OrderBy(cs => cs.FullName)
               .ThenBy(cs => cs.Id)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .To<ClassSocietyBaseServiceModel>();

        public IQueryable<ClassSocietyBaseServiceModel> All() => this.dbContext
                .ClassificationSocieties
                .OrderBy(cs => cs.FullName)
                .To<ClassSocietyBaseServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.ClassificationSocieties.CountAsync();
    }
}
