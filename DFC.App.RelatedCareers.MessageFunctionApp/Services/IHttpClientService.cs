using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.Models.PatchModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public interface IHttpClientService
    {
        Task<HttpStatusCode> PostAsync(RelatedCareersSegmentModel relatedCareersSegmentModel);

        Task<HttpStatusCode> PutAsync(RelatedCareersSegmentModel relatedCareersSegmentModel);

        Task<HttpStatusCode> PatchAsync<T>(T patchModel, string patchTypeEndpoint)
            where T : BasePatchModel;

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}