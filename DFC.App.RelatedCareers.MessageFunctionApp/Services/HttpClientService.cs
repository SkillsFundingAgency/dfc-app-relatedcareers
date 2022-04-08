using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.MessageFunctionApp.Models;
using DFC.Logger.AppInsights.Constants;
using DFC.Logger.AppInsights.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly SegmentClientOptions segmentClientOptions;
        private readonly HttpClient httpClient;
        private readonly ILogService logService;
        private readonly ICorrelationIdProvider correlationIdProvider;

        public HttpClientService(
            SegmentClientOptions segmentClientOptions,
            IHttpClientFactory httpClientFactory,
            ILogService logService,
            ICorrelationIdProvider correlationIdProvider)
        {
            this.segmentClientOptions = segmentClientOptions;
            this.httpClient = httpClientFactory.CreateClient();
            this.logService = logService;
            this.correlationIdProvider = correlationIdProvider;
        }

        public async Task<HttpStatusCode> PostAsync(RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");
            ConfigureHttpClient();

            using (var content = new ObjectContent(typeof(RelatedCareersSegmentModel), relatedCareersSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logService.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for POST, Id: {relatedCareersSegmentModel?.DocumentId}.");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PutAsync(RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");
            ConfigureHttpClient();

            using (var content = new ObjectContent(typeof(RelatedCareersSegmentModel), relatedCareersSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PutAsync(url, content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logService.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Put type {typeof(RelatedCareersSegmentModel)}, Id: {relatedCareersSegmentModel?.DocumentId}");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment/{id}");
            ConfigureHttpClient();

            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                logService.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Delete type {typeof(RelatedCareersSegmentModel)}, Id: {id}");
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode;
        }

        private void ConfigureHttpClient()
        {
            if (!httpClient.DefaultRequestHeaders.Contains(HeaderName.CorrelationId))
            {
                httpClient.DefaultRequestHeaders.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
            }
        }
    }
}