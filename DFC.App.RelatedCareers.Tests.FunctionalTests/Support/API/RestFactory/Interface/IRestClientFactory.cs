using RestSharp;
using System;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API.RestFactory.Interface
{
    public interface IRestClientFactory
    {
        IRestClient Create(Uri baseUrl);
    }
}
