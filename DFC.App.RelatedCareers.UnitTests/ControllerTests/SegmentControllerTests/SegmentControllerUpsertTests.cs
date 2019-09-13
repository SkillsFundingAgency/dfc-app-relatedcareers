using DFC.App.RelatedCareers.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Create or Update Tests")]
    public class SegmentControllerUpsertTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsSuccessForCreate(string mediaTypeName)
        {
            // Arrange
            var relatedCareersSegmentModel = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);
            var expectedUpsertResponse = BuildExpectedUpsertResponse(A.Fake<RelatedCareersSegmentModel>());

            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).Returns(expectedUpsertResponse);

            // Act
            var result = await controller.Save(relatedCareersSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            var relatedCareersSegmentModel = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);
            var expectedUpsertResponse = BuildExpectedUpsertResponse(A.Fake<RelatedCareersSegmentModel>(), HttpStatusCode.OK);

            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).Returns(expectedUpsertResponse);

            // Act
            var result = await controller.Save(relatedCareersSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.UpsertAsync(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.Save(null).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            var relatedCareersSegmentModel = new RelatedCareersSegmentModel();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Save(relatedCareersSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        private UpsertRelatedCareersSegmentModelResponse BuildExpectedUpsertResponse(RelatedCareersSegmentModel model, HttpStatusCode status = HttpStatusCode.Created)
        {
            return new UpsertRelatedCareersSegmentModelResponse
            {
                RelatedCareersSegmentModel = model,
                ResponseStatusCode = status,
            };
        }
    }
}