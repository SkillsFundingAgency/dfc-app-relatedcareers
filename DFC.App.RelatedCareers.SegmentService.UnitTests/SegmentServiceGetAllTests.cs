using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.DraftSegmentService;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using FakeItEasy;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.SegmentService.UnitTests
{
    [Trait("Segment Service", "GetAll Tests")]
    public class SegmentServiceGetAllTests
    {
        private readonly ICosmosRepository<RelatedCareersSegmentModel> repository;
        private readonly IRelatedCareersSegmentService relatedCareersSegmentService;

        public SegmentServiceGetAllTests()
        {
            var draftRelatedCareersSegmentService = A.Fake<IDraftRelatedCareersSegmentService>();
            var mapper = A.Fake<AutoMapper.IMapper>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            repository = A.Fake<ICosmosRepository<RelatedCareersSegmentModel>>();
            relatedCareersSegmentService = new RelatedCareersSegmentService(repository, draftRelatedCareersSegmentService, mapper, jobProfileSegmentRefreshService);
        }

        [Fact]
        public async Task SegmentServiceGetAllListReturnsSuccess()
        {
            // arrange
            var expectedResults = A.CollectionOfFake<RelatedCareersSegmentModel>(2);
            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            // act
            var results = await relatedCareersSegmentService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResults, results);
        }

        [Fact]
        public async Task SegmentServiceGetAllListReturnsNullWhenMissingRepository()
        {
            // arrange
            A.CallTo(() => repository.GetAllAsync()).Returns((IEnumerable<RelatedCareersSegmentModel>)null);

            // act
            var results = await relatedCareersSegmentService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            Assert.Null(results);
        }
    }
}