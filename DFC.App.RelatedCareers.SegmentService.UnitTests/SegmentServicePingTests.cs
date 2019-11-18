using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.DraftSegmentService;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using FakeItEasy;
using Xunit;

namespace DFC.App.RelatedCareers.SegmentService.UnitTests
{
    [Trait("Segment Service", "Ping / Health Tests")]
    public class SegmentServicePingTests
    {
        [Fact]
        public void RelatedCareersSegmentServicePingReturnsSuccess()
        {
            // arrange
            const bool expectedResult = true;
            var repository = A.Fake<ICosmosRepository<RelatedCareersSegmentModel>>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            var mapper = A.Fake<AutoMapper.IMapper>();
            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var relatedCareersSegmentService = new RelatedCareersSegmentService(repository, A.Fake<IDraftRelatedCareersSegmentService>(), mapper, jobProfileSegmentRefreshService);

            // act
            var result = relatedCareersSegmentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void RelatedCareersSegmentServicePingReturnsFalseWhenMissingRepository()
        {
            // arrange
            const bool expectedResult = false;
            var repository = A.Fake<ICosmosRepository<RelatedCareersSegmentModel>>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            var mapper = A.Fake<AutoMapper.IMapper>();
            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var relatedCareersSegmentService = new RelatedCareersSegmentService(repository, A.Fake<IDraftRelatedCareersSegmentService>(), mapper, jobProfileSegmentRefreshService);

            // act
            var result = relatedCareersSegmentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }
    }
}