using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Get Body Tests")]
    public class SegmentControllerGetBodyTests : BaseSegmentController
    {
        private const string Article = "an-article-name";

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void SegmentControllerGetBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Body(Article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<DocumentViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void SegmentControllerGetBodyHtmlReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns((RelatedCareersSegmentModel)null);

            // Act
            var result = await controller.Body(Article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerGetBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var relatedCareersSegmentModel = new RelatedCareersSegmentModel { CanonicalName = "SomeCanonicalName" };
            var fakeDocumentViewModel = new DocumentViewModel { CanonicalName = relatedCareersSegmentModel.CanonicalName };

            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(relatedCareersSegmentModel);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(fakeDocumentViewModel);

            // Act
            var result = await controller.Body(Article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<DocumentViewModel>(jsonResult.Value);

            Assert.True(!string.IsNullOrWhiteSpace(model.CanonicalName) && model.CanonicalName.Equals(relatedCareersSegmentModel.CanonicalName, StringComparison.OrdinalIgnoreCase));

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async void SegmentControllerGetBodyPlainMediaTypeReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Body(Article).ConfigureAwait(false);
            var viewResult = Assert.IsType<StatusCodeResult>(result);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal((int)HttpStatusCode.NotAcceptable, viewResult.StatusCode);
            controller.Dispose();
        }
    }
}