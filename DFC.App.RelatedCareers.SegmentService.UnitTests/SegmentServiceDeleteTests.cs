using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.DraftSegmentService;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.SegmentService.UnitTests
{
    [Trait("Segment Service", "Delete Tests")]
    public class SegmentServiceDeleteTests
    {
        private readonly ICosmosRepository<RelatedCareersSegmentModel> repository;
        private readonly IRelatedCareersSegmentService relatedCareersSegmentService;

        private readonly Guid documentId = Guid.NewGuid();

        public SegmentServiceDeleteTests()
        {
            var draftRelatedCareersSegmentService = A.Fake<IDraftRelatedCareersSegmentService>();
            var mapper = A.Fake<AutoMapper.IMapper>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            repository = A.Fake<ICosmosRepository<RelatedCareersSegmentModel>>();
            relatedCareersSegmentService = new RelatedCareersSegmentService(repository, draftRelatedCareersSegmentService, mapper, jobProfileSegmentRefreshService);
        }

        [Fact]
        public async Task RelatedCareersSegmentServiceDeleteReturnsSuccessWhenSegmentDeleted()
        {
            // arrange
            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.NoContent);

            // act
            var result = await relatedCareersSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            Assert.True(result);
        }

        [Fact]
        public async Task RelatedCareersSegmentServiceDeleteReturnsFalseWhenDocumentNotFound()
        {
            // arrange
            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.NotFound);

            // act
            var result = await relatedCareersSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            Assert.False(result);
        }

        [Fact]
        public async Task RelatedCareersSegmentServiceDeleteReturnsFalseWhenAnyOtherStatus()
        {
            // arrange
            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = await relatedCareersSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            Assert.False(result);
        }
    }
}