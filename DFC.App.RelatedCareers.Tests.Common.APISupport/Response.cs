using RestSharp;
using System.Net;

namespace DFC.App.RelatedCareers.Tests.Common.APISupport
{
    public class Response<T>
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public T Data { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
