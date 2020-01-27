﻿using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    public class SegmentControllerRefreshDocumentsTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task ReturnsSuccessForHtmlMediaType(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);
            var dbModels = GetSegmentModels();

            A.CallTo(() => FakeRelatedCareersSegmentService.GetAllAsync()).Returns(dbModels);

            // Act
            var result = await controller.RefreshDocuments().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobProfileSegmentRefreshService.SendMessageListAsync(A<List<RefreshJobProfileSegmentServiceBusModel>>.Ignored)).MustHaveHappenedOnceExactly();
            var res = Assert.IsType<JsonResult>(result);
            Assert.Equal(dbModels.Count, (res.Value as List<RefreshJobProfileSegmentServiceBusModel>).Count);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task ReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            List<RelatedCareersSegmentModel> expectedResult = null;
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeRelatedCareersSegmentService.GetAllAsync()).Returns(expectedResult);

            // Act
            var result = await controller.RefreshDocuments().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobProfileSegmentRefreshService.SendMessageListAsync(A<List<RefreshJobProfileSegmentServiceBusModel>>.Ignored)).MustNotHaveHappened();
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.CallTo(() => FakeLogger.LogWarning(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        private List<RelatedCareersSegmentModel> GetSegmentModels()
        {
            return new List<RelatedCareersSegmentModel>
            {
                new RelatedCareersSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile1",
                },
                new RelatedCareersSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile2",
                },
                new RelatedCareersSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile3",
                },
                new RelatedCareersSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile4",
                },
                new RelatedCareersSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile5",
                },
            };
        }
    }
}