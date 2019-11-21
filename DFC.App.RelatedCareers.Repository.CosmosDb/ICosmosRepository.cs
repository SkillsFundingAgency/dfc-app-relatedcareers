using DFC.App.RelatedCareers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Repository.CosmosDb
{
    public interface ICosmosRepository<T>
        where T : IDataModel
    {
        Task<bool> PingAsync();

        Task<T> GetAsync(Expression<Func<T, bool>> where);

        Task<IEnumerable<T>> GetAllAsync();

        Task<HttpStatusCode> UpsertAsync(T model);

        Task<HttpStatusCode> DeleteAsync(Guid documentId);
    }
}