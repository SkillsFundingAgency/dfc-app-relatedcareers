using DFC.App.RelatedCareers.Controllers;
using DFC.App.RelatedCareers.SegmentService;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.HealthControllerTests
{
    public class BaseHealthController
    {
        protected IRelatedCareersSegmentService FakeRelatedCareersSegmentService;

        public BaseHealthController()
        {
            FakeLogger = A.Fake<ILogger<HealthController>>();
            FakeRelatedCareersSegmentService = A.Fake<IRelatedCareersSegmentService>();
        }

        protected ILogger<HealthController> FakeLogger { get; }

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