﻿using AutoMapper;
using DFC.App.RelatedCareers.MessageFunctionApp.Models;
using DFC.App.RelatedCareers.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard;
using DFC.Logger.AppInsights.Contracts;
using DFC.Logger.AppInsights.CorrelationIdProviders;
using DFC.Logger.AppInsights.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

[assembly: WebJobsStartup(typeof(DFC.App.RelatedCareers.MessageFunctionApp.Startup.WebJobsExtensionStartup), "Web Jobs Extension Startup")]

namespace DFC.App.RelatedCareers.MessageFunctionApp.Startup
{
    [ExcludeFromCodeCoverage]
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var segmentClientOptions = configuration.GetSection("RelatedCareersSegmentClientOptions").Get<SegmentClientOptions>();

            builder.AddDependencyInjection();
            builder?.Services.AddAutoMapper(typeof(WebJobsExtensionStartup).Assembly);
            builder.Services.AddSingleton(segmentClientOptions);
            builder?.Services.AddHttpClient();
            builder?.Services.AddScoped<IHttpClientService, HttpClientService>();
            builder?.Services.AddScoped<IMessageProcessor, MessageProcessor>();
            builder?.Services.AddScoped<IMappingService, MappingService>();
            builder?.Services.AddScoped<IMessagePropertiesService, MessagePropertiesService>();
            builder?.Services.AddDFCLogging(configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
            builder?.Services.AddScoped<ICorrelationIdProvider, InMemoryCorrelationIdProvider>();
        }
    }
}