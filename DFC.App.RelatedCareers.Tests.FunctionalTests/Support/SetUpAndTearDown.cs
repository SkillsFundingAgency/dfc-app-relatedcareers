﻿using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.ContentType.JobProfile;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.Support;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API.RestFactory;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.AzureServiceBus;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.AzureServiceBus.ServiceBusFactory;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support
{
    public class SetUpAndTearDown
    {
        protected CommonAction CommonAction { get; set; } = new CommonAction();

        protected AppSettings AppSettings { get; set; }

        protected JobProfileContentType WakeUpJobProfile { get; set; }

        protected JobProfileContentType JobProfile { get; set; }

        protected ServiceBusSupport ServiceBus { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Get settings from appsettings
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            this.AppSettings = configuration.Get<AppSettings>();

            this.ServiceBus = new ServiceBusSupport(new TopicClientFactory(), this.AppSettings);

            // Send wake up job profile
            this.WakeUpJobProfile = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            this.WakeUpJobProfile.JobProfileId = Guid.NewGuid().ToString();
            this.WakeUpJobProfile.CanonicalName = this.CommonAction.RandomString(10).ToLowerInvariant();
            var jobProfileMessageBody = this.CommonAction.ConvertObjectToByteArray(this.WakeUpJobProfile);
            var message = new MessageFactory().Create(this.WakeUpJobProfile.JobProfileId, jobProfileMessageBody, "Published", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromMinutes(this.AppSettings.DeploymentWaitInMinutes)).ConfigureAwait(true);

            // Generate a test job profile
            var relatedCareersData = new RelatedCareersData()
            {
                Id = Guid.NewGuid().ToString(),
                ProfileLink = "/related-careers-link",
                Title = "This is the related careers title",
            };

            this.JobProfile = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            this.JobProfile.JobProfileId = Guid.NewGuid().ToString();
            this.JobProfile.CanonicalName = this.CommonAction.RandomString(10).ToLowerInvariant();
            this.JobProfile.RelatedCareersData = new List<RelatedCareersData>() { relatedCareersData };

            // Send job profile to the service bus
            jobProfileMessageBody = this.CommonAction.ConvertObjectToByteArray(this.JobProfile);
            message = new MessageFactory().Create(this.JobProfile.JobProfileId, jobProfileMessageBody, "Published", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(10000).ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            // Delete wake up job profile
            var wakeUpJobProfileDelete = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            wakeUpJobProfileDelete.JobProfileId = this.WakeUpJobProfile.JobProfileId;
            var messageBody = this.CommonAction.ConvertObjectToByteArray(wakeUpJobProfileDelete);
            var message = new MessageFactory().Create(this.WakeUpJobProfile.JobProfileId, messageBody, "Deleted", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);

            // Delete test job profile
            var jobProfileDelete = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            jobProfileDelete.JobProfileId = this.JobProfile.JobProfileId;
            messageBody = this.CommonAction.ConvertObjectToByteArray(jobProfileDelete);
            message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Deleted", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
        }
    }
}
