using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.MessageFunctionApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;
using DFC.App.RelatedCareers.Data.Models.PatchModels;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly SegmentClientOptions segmentClientOptions;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public HttpClientService(SegmentClientOptions segmentClientOptions, HttpClient httpClient, ILogger logger)
        {
            this.segmentClientOptions = segmentClientOptions;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<HttpStatusCode> PostAsync(RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");

            using (var content = new ObjectContent(typeof(RelatedCareersSegmentModel), relatedCareersSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PutAsync(RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");

            using (var content = new ObjectContent(typeof(RelatedCareersSegmentModel), relatedCareersSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PutAsync(url, content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Put type {typeof(RelatedCareersSegmentModel)}, Id: {relatedCareersSegmentModel?.DocumentId}");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PatchAsync<T>(T patchModel, string patchTypeEndpoint)
            where T : BasePatchModel
        {
            var url = new Uri($"{segmentClientOptions.BaseAddress}segment/{patchModel?.JobProfileId}/{patchTypeEndpoint}");
            using (var content = new ObjectContent<T>(patchModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PatchAsync(url, content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for patch type {typeof(T)}, Id: {patchModel.JobProfileId}");

                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment/{id}");
            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Failure status code '{response.StatusCode}' received for Deleting Job Profile with Id: {id}");
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode;
        }
    }
}