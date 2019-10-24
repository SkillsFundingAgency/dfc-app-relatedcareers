﻿using AutoMapper;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.DraftSegmentService;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using DFC.App.RelatedCareers.Repository.SitefinityApi;
using DFC.App.RelatedCareers.SegmentService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DFC.App.RelatedCareers
{
    public class Startup
    {
        public const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:JobProfileSegment";
        public const string SitefinityApiAppSettings = "SitefinityApi";
        public const string ServiceBusOptionsAppSettings = "ServiceBusOptions";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var serviceBusOptions = configuration.GetSection(ServiceBusOptionsAppSettings).Get<ServiceBusOptions>();
            services.AddSingleton(serviceBusOptions ?? new ServiceBusOptions());


            var cosmosDbConnection = configuration.GetSection(CosmosDbConfigAppSettings).Get<CosmosDbConnection>();
            var documentClient = new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey);
            var sitefinityApiConnection = configuration.GetSection(SitefinityApiAppSettings).Get<SitefinityAPIConnectionSettings>();

            services.AddSingleton(sitefinityApiConnection ?? new SitefinityAPIConnectionSettings());
            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<RelatedCareersSegmentModel>, CosmosRepository<RelatedCareersSegmentModel>>();
            services.AddScoped<IRelatedCareersSegmentService, RelatedCareersSegmentService>();
            services.AddScoped<IDraftRelatedCareersSegmentService, DraftRelatedCareersSegmentService>();
            services.AddSingleton<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>, JobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvcWithDefaultRoute();

            mapper?.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}