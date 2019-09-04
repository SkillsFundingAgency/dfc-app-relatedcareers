using DFC.App.RelatedCareers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Repository.CosmosDb
{
    public interface ICosmosRepository<T>
        where T : IDataModel
    {
        Task<T> GetAsync(Expression<Func<T, bool>> where);

        Task<IEnumerable<T>> GetAllAsync();
    }
}