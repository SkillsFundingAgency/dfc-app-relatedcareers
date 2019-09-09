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
        private const string JobCanonicalName = "relatedJob1";

        [Fact]
        public void ContainsContentFromModel()
        {
            var expectedRelatedCareerMarkup = $"<li><a href=\"../{JobCanonicalName}\">{JobTitle}</a></li>";

            var model = new DocumentViewModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "nurse",
                Data = new DocumentDataViewModel
                {
                    RelatedCareers = new List<RelatedCareerDataViewModel>
                    {
                        new RelatedCareerDataViewModel
                        {
                            CanonicalName = JobCanonicalName,
                            Title = JobTitle,
                        },
                    },
                },
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            var viewRenderResponse = viewRenderer.Render(@"Body", model, viewBag);

            Assert.Contains(expectedRelatedCareerMarkup, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
        }
    }
}