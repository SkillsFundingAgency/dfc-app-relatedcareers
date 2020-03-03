using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Delete Tests")]
    public class SegmentControllerDeleteTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerDeleteReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const bool documentExists = true;
            var controller = BuildSegmentController(mediaTypeName);
            A.CallTo(() => FakeRelatedCareersSegmentService.DeleteAsync(A<Guid>.Ignored)).Returns(documentExists);

            // Act
            var result = await controller.Delete(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerDeleteReturnsNotFound(string mediaTypeName)
        {
            // Arrange
            const bool documentExists = false;
            var controller = BuildSegmentController(mediaTypeName);
            A.CallTo(() => FakeRelatedCareersSegmentService.DeleteAsync(A<Guid>.Ignored)).Returns(documentExists);

            // Act
            var result = await controller.Delete(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}