﻿using DFC.App.RelatedCareers.Controllers;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.SegmentService;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.RelatedCareers.UnitTests.ControllerTests.SegmentControllerTests
{
    public class BaseSegmentController
    {
        public BaseSegmentController()
        {
            FakeLogger = A.Fake<ILogService>();
            FakeRelatedCareersSegmentService = A.Fake<IRelatedCareersSegmentService>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeJobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new object[] { "*/*" },
            new object[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> InvalidMediaTypes => new List<object[]>
        {
            new object[] { MediaTypeNames.Text.Plain },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new object[] { MediaTypeNames.Application.Json },
        };

        protected ILogService FakeLogger { get; }

        protected IRelatedCareersSegmentService FakeRelatedCareersSegmentService { get; }

        protected IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> FakeJobProfileSegmentRefreshService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected SegmentController BuildSegmentController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new SegmentController(FakeLogger, FakeRelatedCareersSegmentService, FakeMapper, FakeJobProfileSegmentRefreshService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}