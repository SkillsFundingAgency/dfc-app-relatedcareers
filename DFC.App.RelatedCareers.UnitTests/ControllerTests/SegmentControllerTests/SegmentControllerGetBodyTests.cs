using DFC.App.RelatedCareers.ApiModels;
using DFC.App.RelatedCareers.Controllers;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Get Body Tests")]
    public class SegmentControllerGetBodyTests : BaseSegmentController
    {
        private readonly Guid documentId = Guid.NewGuid();

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void SegmentControllerGetBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            var model = new DocumentViewModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "nurse",
                RoutePrefix = SegmentController.SegmentRoutePrefix,
                Data = new DocumentDataViewModel
                {
                    LastReviewed = DateTime.UtcNow,
                    RelatedCareers = new List<RelatedCareerDataViewModel>
                    {
                        new RelatedCareerDataViewModel
                        {
                            Id = Guid.NewGuid(),
                            ProfileLink = "related-job-1",
                            Title = "RelatedJobTitle1",
                        },
                    },
                },
            };

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(model);

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
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
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns((RelatedCareersSegmentModel)null);

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
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
            var fakeRelatedCareerApiModel = new List<RelatedCareerApiModel>
            {
                new RelatedCareerApiModel
                {
                    Title = "SomeTitle1",
                    Url = "someurl1",
                },
            };

            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(relatedCareersSegmentModel);
            A.CallTo(() => FakeMapper.Map<List<RelatedCareerApiModel>>(A<RelatedCareersSegmentModel>.Ignored)).Returns(fakeRelatedCareerApiModel);

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<List<RelatedCareerApiModel>>(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<List<RelatedCareerApiModel>>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async void SegmentControllerGetBodyPlainMediaTypeReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<RelatedCareersSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);
            var viewResult = Assert.IsType<StatusCodeResult>(result);

            // Assert
            A.CallTo(() => FakeRelatedCareersSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<RelatedCareersSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal((int)HttpStatusCode.NotAcceptable, viewResult.StatusCode);
            controller.Dispose();
        }
    }
}