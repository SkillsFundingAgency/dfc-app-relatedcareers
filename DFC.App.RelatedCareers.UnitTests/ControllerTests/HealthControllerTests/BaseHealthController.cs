﻿using DFC.App.RelatedCareers.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.HealthControllerTests
{
    public class BaseHealthController
    {
        public BaseHealthController()
        {
            FakeLogger = A.Fake<ILogger<HealthController>>();
        }

        protected ILogger<HealthController> FakeLogger { get; }

        protected HealthController BuildHealthController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            return new HealthController(FakeLogger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}