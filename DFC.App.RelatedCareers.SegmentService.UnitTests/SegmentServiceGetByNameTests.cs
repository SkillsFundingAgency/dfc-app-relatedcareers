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
    [Trait("Segment Service", "GetByName Tests")]
    public class SegmentServiceGetByNameTests
    {
        private readonly ICosmosRepository<RelatedCareersSegmentModel> repository;
        private readonly IRelatedCareersSegmentService relatedCareersSegmentService;

        public SegmentServiceGetByNameTests()
        {
            var mapper = A.Fake<AutoMapper.IMapper>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            repository = A.Fake<ICosmosRepository<RelatedCareersSegmentModel>>();
            relatedCareersSegmentService = new RelatedCareersSegmentService(repository, mapper, jobProfileSegmentRefreshService);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<RelatedCareersSegmentModel>();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await relatedCareersSegmentService.GetByNameAsync("article-name").ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await relatedCareersSegmentService.GetByNameAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: canonicalName", exceptionResult.Message);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsNullWhenMissingInRepository()
        {
            // arrange
            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).Returns((RelatedCareersSegmentModel)null);

            // act
            var result = await relatedCareersSegmentService.GetByNameAsync("article-name").ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<RelatedCareersSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }
    }
}