using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.API;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API
{
    public interface IRelatedCareersAPI
    {
        Task<IRestResponse<List<RelatedCareersResponse>>> GetById(string id);
    }
}
