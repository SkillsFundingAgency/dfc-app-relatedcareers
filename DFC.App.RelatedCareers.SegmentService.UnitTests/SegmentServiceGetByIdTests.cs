using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.SegmentService.UnitTests
{
    [Trait("Segment Service", "GetById Tests")]
    public class SegmentServiceGetByIdTests
    {
        private readonly ICosmosRepository<RelatedCareersSegmentModel> repository;
        private readonly IRelatedCareersSegmentService relatedCareersSegmentService;

        public SegmentServiceGetByIdTests()
        {
            var mapper = A.Fake<AutoMapper.IMapper>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            repository = A.Fake<ICosmosRepository<RelatedCareersSegmentModel>>();
            relatedCareersSegmentService = new RelatedCareersSegmentService(repository, mapper, jobProfileSegmentRefreshService);
        }

        [Fact]
        public async Task SegmentServiceGetByIdReturnsSuccess()
        {
            // arrange
            var documentId = Guid.NewGuid();
            var expectedResult = A.Fake<RelatedCareersSegmentModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await relatedCareersSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task SegmentServiceGetByIdReturnsNullWhenMissingInRepository()
        {
            // arrange
            var documentId = Guid.NewGuid();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).Returns((RelatedCareersSegmentModel)null);

            // act
            var result = await relatedCareersSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }
    }
}