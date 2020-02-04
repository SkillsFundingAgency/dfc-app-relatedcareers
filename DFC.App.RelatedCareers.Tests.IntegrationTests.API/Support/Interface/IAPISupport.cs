using DFC.App.RelatedCareers.Tests.Common.APISupport;
using System.Threading.Tasks;
using static DFC.App.RelatedCareers.Tests.Common.APISupport.GetRequest;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    interface IAPISupport
    {
        Task<Response<T>> ExecuteGetRequest<T>(string endpoint, ContentType responseFormat, bool AuthoriseRequest = true);
    }
}
