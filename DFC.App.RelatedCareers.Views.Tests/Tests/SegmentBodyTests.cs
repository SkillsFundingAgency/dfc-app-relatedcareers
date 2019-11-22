using DFC.App.RelatedCareers.Controllers;
using DFC.App.RelatedCareers.ViewModels;
using DFC.App.RelatedCareers.Views.Tests.ViewRenderer;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.App.RelatedCareers.Views.Tests.Tests
{
    public class SegmentBodyTests : TestsBase
    {
        private const string JobTitle = "Related Job 1 Title";
        private const string JobCanonicalName = "/relatedJob1";

        [Fact]
        public void ContainsContentFromModel()
        {
            // Arrange
            var expectedRelatedCareerMarkup = $"<li><a href=\"/{SegmentController.SegmentRoutePrefix}{JobCanonicalName}\">{JobTitle}</a></li>";

            var model = new DocumentViewModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "nurse",
                RoutePrefix = SegmentController.SegmentRoutePrefix,
                Data = new DocumentDataViewModel
                {
                    RelatedCareers = new List<RelatedCareerDataViewModel>
                    {
                        new RelatedCareerDataViewModel
                        {
                            ProfileLink = JobCanonicalName,
                            Title = JobTitle,
                        },
                    },
                },
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(ViewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"Body", model, viewBag);

            // Assert
            Assert.Contains(expectedRelatedCareerMarkup, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
        }
    }
}