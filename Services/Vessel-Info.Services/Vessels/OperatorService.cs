namespace Vessel_Info.Services.Vessels
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Operators;

    public class OperatorService : IOperatorService
    {
        private readonly VesselInfoDbContext dbContext;

        public OperatorService(VesselInfoDbContext dbContext) => this.dbContext = dbContext;

        public async Task<OperatorAllServiceModel> GetByIdAsync(int? id) => await this.dbContext
                .Operators
                .Where(o => o.Id == id)
                .To<OperatorAllServiceModel>()
                .FirstOrDefaultAsync();

        public IQueryable<OperatorAllServiceModel> GetAllBySearchTerm(string searchTerm) => this.dbContext
                .Operators
                .Where(v => v.Name.StartsWith(searchTerm))
                .OrderBy(v => v.Name)
                .ThenBy(v => v.Id)
                .To<OperatorAllServiceModel>();

        public async Task<int> GetOrCreateOperatorAsync(string operatorName)
        {
            var @operator = await this.dbContext
                .Operators
                .FirstOrDefaultAsync(x => x.Name == operatorName);

            if (@operator != null)
            {
                return @operator.Id;
            }

            @operator = new Operator
            {
                Name = operatorName
            };

            await this.dbContext.Operators.AddAsync(@operator);
            await this.dbContext.SaveChangesAsync();

            return @operator.Id;
        }

        public async Task<OperatorDetailsServiceModel> DetailsAsync(int? id)
        {
            var details = await this.dbContext
                .Operators
                .Where(o => o.Id == id)
                .To<OperatorDetailsServiceModel>()
                .FirstOrDefaultAsync();

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }

        public async Task<bool> EditAsync(int? id, OperatorEditServiceModel model)
        {
            var edit = await this.dbContext.Operators.FindAsync(id);

            if (edit == null)
            {
                return false;
            }

            edit.Name = model.Name;
            edit.Founded = model.Founded;
            edit.Website = model.Website;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> FindOperatorIdByNameAsync(string vesselOperator) => await this.dbContext
                .Operators
                .Where(o => o.Name == vesselOperator)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();

        public IQueryable<OperatorBaseServiceModel> AllPaging(int page, int pageSize = 10) => this.dbContext
                .Operators
                .OrderBy(o => o.Name)
                .ThenBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .To<OperatorBaseServiceModel>();

        public IQueryable<OperatorBaseServiceModel> All() => this.dbContext
                .Operators
                .OrderBy(o => o.Name)
                .To<OperatorBaseServiceModel>();

        public async Task<int> GetCountAsync() => await this.dbContext.Operators.CountAsync();
    }
}
