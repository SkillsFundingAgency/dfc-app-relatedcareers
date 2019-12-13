using DFC.App.RelatedCareers.Controllers;
using DFC.App.RelatedCareers.SegmentService;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.HealthControllerTests
{
    public class BaseHealthController
    {
        protected IRelatedCareersSegmentService FakeRelatedCareersSegmentService;

        public BaseHealthController()
        {
            FakeLogger = A.Fake<ILogService>();
            FakeRelatedCareersSegmentService = A.Fake<IRelatedCareersSegmentService>();
        }

        protected ILogService FakeLogger { get; }

        protected HealthController BuildHealthController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            return new HealthController(FakeLogger, FakeRelatedCareersSegmentService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}