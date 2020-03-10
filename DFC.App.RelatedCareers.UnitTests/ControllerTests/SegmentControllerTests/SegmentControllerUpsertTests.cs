using DFC.App.RelatedCareers.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Create or Update Tests")]
    public class SegmentControllerUpsertTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerUpsertReturnsSuccessForCreate(string mediaTypeName)
        {
            // Arrange
            var relatedCareersSegmentModel = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);
            var expectedUpsertResponse = HttpStatusCode.Created;

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns((RelatedCareersSegmentModel)null);
            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).Returns(expectedUpsertResponse);

            // Act
            var result = await controller.Post(relatedCareersSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerUpsertReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            var existingModel = A.Fake<RelatedCareersSegmentModel>();
            existingModel.SequenceNumber = 123;

            var modelToUpsert = A.Fake<RelatedCareersSegmentModel>();
            modelToUpsert.SequenceNumber = 124;

            var controller = BuildSegmentController(mediaTypeName);
            var expectedUpsertResponse = HttpStatusCode.OK;

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingModel);
            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).Returns(expectedUpsertResponse);

            // Act
            var result = await controller.Put(modelToUpsert).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerUpsertReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.Put(null).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerUpsertReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            var relatedCareersSegmentModel = new RelatedCareersSegmentModel();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Put(relatedCareersSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}