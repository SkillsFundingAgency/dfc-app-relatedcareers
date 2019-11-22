using System.Net.Http;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}