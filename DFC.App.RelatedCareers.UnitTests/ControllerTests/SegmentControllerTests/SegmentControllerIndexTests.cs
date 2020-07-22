using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Index Tests")]
    public class SegmentControllerIndexTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task SegmentControllerIndexHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 2;
            var expectedResults = A.CollectionOfFake<RelatedCareersSegmentModel>(resultsCount);
            var controller = BuildSegmentController(mediaTypeName);
            var expectedIndexDocumentViewModel = new IndexDocumentViewModel { CanonicalName = "test-canonical-name" };

            A.CallTo(() => FakeRelatedCareersSegmentService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(expectedIndexDocumentViewModel);

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);

            Assert.Equal(resultsCount, model.Documents.Count());

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerIndexHtmlReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);
            var expectedIndexDocumentViewModel = new IndexDocumentViewModel { CanonicalName = "test-canonical-name" };

            A.CallTo(() => FakeRelatedCareersSegmentService.GetAllAsync()).Returns((IEnumerable<RelatedCareersSegmentModel>)null);
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(expectedIndexDocumentViewModel);

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);

            Assert.Null(model.Documents);

            controller.Dispose();
        }
    }
}