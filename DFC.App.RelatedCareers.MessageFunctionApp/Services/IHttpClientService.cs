using DFC.App.RelatedCareers.Data.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public interface IHttpClientService
    {
        Task<HttpStatusCode> PostAsync(RelatedCareersSegmentModel relatedCareersSegmentModel);

        Task<HttpStatusCode> PutAsync(RelatedCareersSegmentModel relatedCareersSegmentModel);

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}