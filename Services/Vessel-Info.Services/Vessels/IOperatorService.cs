﻿namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Operators;

    public interface IOperatorService
    {
        Task<OperatorAllServiceModel> GetById(int? id);

        Task<int> GetOrCreateOperatorAsync(string operatorName);

        Task<OperatorDetailsServiceModel> DetailsAsync(int? id);

        Task<bool> EditAsync(int? id, OperatorEditServiceModel model);

        Task<int> FindOperatorIdByName(string vesselOperator);

        IQueryable<OperatorBaseServiceModel> AllPaging(int page, int itemsPerPage = 12);

        IQueryable<OperatorBaseServiceModel> All();

        Task<int> GetCountAsync();
    }
}