﻿using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.SegmentService.UnitTests
{
    [Trait("Profile Service", "Update Tests")]
    public class SegmentServiceUpsertTests
    {
        private readonly ICosmosRepository<RelatedCareersSegmentModel> repository;
        private readonly IRelatedCareersSegmentService relatedCareersSegmentService;

        public SegmentServiceUpsertTests()
        {
            var mapper = A.Fake<AutoMapper.IMapper>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            repository = A.Fake<ICosmosRepository<RelatedCareersSegmentModel>>();
            relatedCareersSegmentService = new RelatedCareersSegmentService(repository, mapper, jobProfileSegmentRefreshService);
        }

        [Fact]
        public async Task RelatedCareerSegmentServiceUpsertReturnsCreatedWhenDocumentCreated()
        {
            // arrange
            var relatedCareersSegmentModel = A.Fake<RelatedCareersSegmentModel>();
            var expectedResult = HttpStatusCode.Created;

            A.CallTo(() => repository.UpsertAsync(relatedCareersSegmentModel)).Returns(expectedResult);

            // act
            var result = await relatedCareersSegmentService.UpsertAsync(relatedCareersSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(relatedCareersSegmentModel)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task RelatedCareerSegmentServiceUpsertReturnsSuccessWhenDocumentReplaced()
        {
            // arrange
            var relatedCareersSegmentModel = A.Fake<RelatedCareersSegmentModel>();
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => repository.UpsertAsync(relatedCareersSegmentModel)).Returns(expectedResult);

            // act
            var result = await relatedCareersSegmentService.UpsertAsync(relatedCareersSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(relatedCareersSegmentModel)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task RelatedCareerSegmentServiceUpsertReturnsArgumentNullExceptionWhenNullParamIsUsed()
        {
            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await relatedCareersSegmentService.UpsertAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'relatedCareersSegmentModel')", exceptionResult.Message);
        }
    }
}