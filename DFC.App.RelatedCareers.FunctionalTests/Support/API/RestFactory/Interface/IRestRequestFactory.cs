﻿using RestSharp;
using System;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API.RestFactory.Interface
{
    public interface IRestRequestFactory
    {
        IRestRequest Create(string urlSuffix);
    }
}
