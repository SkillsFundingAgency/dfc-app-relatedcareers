using DFC.App.RelatedCareers.Data.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.IntegrationTests.ControllerTests
{
    [Trait("Integration Tests", "Segment Controller Tests")]
    public class SegmentControllerRouteTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DataSeeding>
    {
        private const string SegmentUrl = "/segment";
        private readonly CustomWebApplicationFactory<Startup> factory;

        public SegmentControllerRouteTests(CustomWebApplicationFactory<Startup> factory, DataSeeding seeding)
        {
            this.factory = factory;
            seeding?.SeedDefaultArticle(factory).GetAwaiter().GetResult();
        }

        public static IEnumerable<object[]> SegmentDocumentRouteData => new List<object[]>
        {
            new object[] { SegmentUrl },
            new object[] { $"{SegmentUrl}/{DataSeeding.Job1CanonicalName}" },
        };

        public static IEnumerable<object[]> MissingSegmentContentRouteData => new List<object[]>
        {
            new object[] { $"{SegmentUrl}/invalid-segment-name" },
        };

        public static IEnumerable<object[]> SegmentBodyRouteData => new List<object[]>
        {
            new object[] { $"{SegmentUrl}/{DataSeeding.MainArticleGuid}/contents", MediaTypeNames.Application.Json },
            new object[] { $"{SegmentUrl}/{DataSeeding.MainArticleGuid}/contents", MediaTypeNames.Text.Html },
        };

        [Theory]
        [MemberData(nameof(SegmentDocumentRouteData))]
        public async Task GetSegmentDocumentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Text.Html}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(MissingSegmentContentRouteData))]
        public async Task GetSegmentDocumentEndpointsReturnNoContent(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(SegmentBodyRouteData))]
        public async Task GetSegmentBodyEndpointReturnsSuccessAndCorrectContentType(string url, string mediaType)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{mediaType}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task PostSegmentEndpointsReturnCreated()
        {
            // Arrange
            var relatedCareersSegmentModel = new RelatedCareersSegmentModel()
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = Guid.NewGuid().ToString(),
                SocLevelTwo = "12PostSoc",
                Data = new RelatedCareerSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                    RelatedCareers = new List<RelatedCareerDataModel>
                    {
                        new RelatedCareerDataModel
                        {
                            ProfileLink = "relatedJobName",
                            Id = Guid.NewGuid(),
                            Title = "relatedJobTitle",
                        },
                    },
                },
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(SegmentUrl, relatedCareersSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostWhenUpdateExistingArticleReturnsAlreadyReported()
        {
            // Arrange
            var relatedCareersSegmentModel = new RelatedCareersSegmentModel()
            {
                DocumentId = DataSeeding.MainArticleGuid,
                CanonicalName = DataSeeding.Job1CanonicalName,
                SocLevelTwo = DataSeeding.MainJobSocLevelTwo,
                Data = new RelatedCareerSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                    RelatedCareers = new List<RelatedCareerDataModel>
                    {
                        new RelatedCareerDataModel
                        {
                            ProfileLink = "relatedJobName",
                            Id = Guid.NewGuid(),
                            Title = "relatedJobTitle",
                        },
                    },
                },
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(SegmentUrl, relatedCareersSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.AlreadyReported, response.StatusCode);
        }

        [Fact]
        public async Task PutSegmentEndpointsReturnOk()
        {
            // Arrange
            var relatedCareersSegmentModel = new RelatedCareersSegmentModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = Guid.NewGuid().ToString(),
                SocLevelTwo = "11PutSoc",
                Data = new RelatedCareerSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                    RelatedCareers = new List<RelatedCareerDataModel>
                    {
                        new RelatedCareerDataModel
                        {
                            ProfileLink = "relatedJobName",
                            Id = Guid.NewGuid(),
                            Title = "relatedJobTitle",
                        },
                    },
                },
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(SegmentUrl, relatedCareersSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);
            relatedCareersSegmentModel.SequenceNumber = 123;

            // Act
            var response = await client.PutAsync(SegmentUrl, relatedCareersSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSegmentEndpointsReturnSuccessWhenFound()
        {
            // Arrange
            var documentId = Guid.NewGuid();

            var deleteUri = new Uri($"{SegmentUrl}/{documentId}", UriKind.Relative);

            var relatedCareersSegmentModel = new RelatedCareersSegmentModel()
            {
                DocumentId = documentId,
                CanonicalName = documentId.ToString().ToLowerInvariant(),
                SocLevelTwo = "12345",
                Data = new RelatedCareerSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                    RelatedCareers = new List<RelatedCareerDataModel>
                    {
                        new RelatedCareerDataModel
                        {
                            ProfileLink = "relatedJobName",
                            Id = Guid.NewGuid(),
                            Title = "relatedJobTitle",
                        },
                    },
                },
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(SegmentUrl, relatedCareersSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Act
            var response = await client.DeleteAsync(deleteUri).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSegmentEndpointsReturnNotFound()
        {
            // Arrange
            var deleteUri = new Uri($"{SegmentUrl}/{Guid.NewGuid()}", UriKind.Relative);
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.DeleteAsync(deleteUri).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}