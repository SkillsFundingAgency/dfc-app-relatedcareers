using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Document Tests")]
    public class SegmentControllerDocumentTests : BaseSegmentController
    {
        private const string Article = "an-article-name";

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task SegmentControllerDocumentHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(Article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<DocumentViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task SegmentControllerDocumentHtmlReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored)).Returns((RelatedCareersSegmentModel)null);

            // Act
            var result = await controller.Document(Article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}