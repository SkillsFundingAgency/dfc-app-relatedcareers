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
            HttpClient httpClient,
            ILogService logService,
            ICorrelationIdProvider correlationIdProvider)
        {
            this.segmentClientOptions = segmentClientOptions;
            this.httpClient = httpClient;
            this.logService = logService;
            this.correlationIdProvider = correlationIdProvider;
        }

        public async Task<HttpStatusCode> PostAsync(RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            logService.LogInformation($"{nameof(PostAsync)} has been called with {nameof(relatedCareersSegmentModel)}");

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

                logService.LogInformation($"{nameof(PostAsync)} reponse returning {response.StatusCode}");
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PutAsync(RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            logService.LogInformation($"{nameof(PutAsync)} has been called with {nameof(relatedCareersSegmentModel)}");

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

                logService.LogInformation($"{nameof(PutAsync)} response returning {response.StatusCode}");
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            logService.LogInformation($"{nameof(DeleteAsync)} has been called with {nameof(id)}");

            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment/{id}");
            ConfigureHttpClient();

            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                logService.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Delete type {typeof(RelatedCareersSegmentModel)}, Id: {id}");
                response.EnsureSuccessStatusCode();
            }

            logService.LogInformation($"{nameof(DeleteAsync)} reponse returning {response.StatusCode}");
            return response.StatusCode;
        }

        private void ConfigureHttpClient()
        {
            logService.LogInformation($"{nameof(ConfigureHttpClient)} has been called");

            if (!httpClient.DefaultRequestHeaders.Contains(HeaderName.CorrelationId))
            {
                httpClient.DefaultRequestHeaders.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
            }

            logService.LogInformation($"{nameof(ConfigureHttpClient)} has been configured");
        }
    }
}