using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.API;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.Support;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API.RestFactory.Interface;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API
{
    public class RelatedCareersAPI : IRelatedCareersAPI
    {
        private IRestClientFactory restClientFactory;
        private IRestRequestFactory restRequestFactory;
        private AppSettings appSettings;

        public RelatedCareersAPI(IRestClientFactory restClientFactory, IRestRequestFactory restRequestFactory, AppSettings appSettings)
        {
            this.restClientFactory = restClientFactory;
            this.restRequestFactory = restRequestFactory;
            this.appSettings = appSettings;
        }

        public async Task<IRestResponse<List<RelatedCareersResponse>>> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var restClient = this.restClientFactory.Create(this.appSettings.APIConfig.EndpointBaseUrl);
            var restRequest = this.restRequestFactory.Create($"/segment/{id}/contents");
            restRequest.AddHeader("Accept", "application/json");
            return await Task.Run(() => restClient.Execute<List<RelatedCareersResponse>>(restRequest)).ConfigureAwait(false);
        }
    }
}
