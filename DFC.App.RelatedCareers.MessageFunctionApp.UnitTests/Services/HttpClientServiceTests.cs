using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.MessageFunctionApp.Models;
using DFC.App.RelatedCareers.MessageFunctionApp.Services;
using DFC.App.RelatedCareers.MessageFunctionApp.UnitTests.ClientHandlers;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.MessageFunctionAppTests.Services
{
    public class HttpClientServiceTests
    {
        private readonly SegmentClientOptions options;
        private readonly ILogService logService;
        private readonly ICorrelationIdProvider correlationIdProvider;

        public HttpClientServiceTests()
        {
            options = new SegmentClientOptions
            {
                BaseAddress = new Uri("http://baseAddress"),
                Timeout = TimeSpan.MinValue,
            };

            logService = A.Fake<ILogService>();
            correlationIdProvider = A.Fake<ICorrelationIdProvider>();
        }

        [Fact]
        public async Task PostAsyncReturnsOKStatusCodeWhenHttpResponseIsSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, httpClient, logService, correlationIdProvider);

            // Act
            var result = await httpClientService.PostAsync(GetSegmentModel()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PostAsyncReturnsExceptionWhenHttpResponseIsNotSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, httpClient, logService, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.PostAsync(GetSegmentModel()).ConfigureAwait(false)).ConfigureAwait(false);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PutAsyncReturnsExceptionWhenHttpResponseIsNotSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, httpClient, logService, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.PutAsync(GetSegmentModel()).ConfigureAwait(false)).ConfigureAwait(false);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PutAsyncReturnsStatusWhenHttpResponseIsNotFound()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, httpClient, logService, correlationIdProvider);

            // Act
            var result = await httpClientService.PutAsync(GetSegmentModel()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PutAsyncReturnsOKStatusCodeWhenHttpResponseIsSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, httpClient, logService, correlationIdProvider);

            // Act
            var result = await httpClientService.PutAsync(GetSegmentModel()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task DeleteAsyncReturnsOKStatusCodeWhenHttpResponseIsSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, httpClient, logService, correlationIdProvider);

            // Act
            var result = await httpClientService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task DeleteAsyncReturnsExceptionWhenHttpResponseIsNotSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, httpClient, logService, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false)).ConfigureAwait(false);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        private RelatedCareersSegmentModel GetSegmentModel()
        {
            return new RelatedCareersSegmentModel
            {
                DocumentId = Guid.NewGuid(),
                SequenceNumber = 1,
                SocLevelTwo = "12",
                CanonicalName = "job-1",
                Data = new RelatedCareerSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                    RelatedCareers = new List<RelatedCareerDataModel>
                    {
                        new RelatedCareerDataModel
                        {
                            Id = Guid.NewGuid(),
                            Title = "RelatedJob1",
                            ProfileLink = "related-job-1",
                        },
                        new RelatedCareerDataModel
                        {
                            Id = Guid.NewGuid(),
                            Title = "RelatedJob2",
                            ProfileLink = "related-job-2",
                        },
                    },
                },
            };
        }
    }
}