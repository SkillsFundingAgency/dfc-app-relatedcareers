using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface;
using System;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support
{
    internal partial class CommonAction : IRelatedCareersSupport
    {
        public RelatedCareersData GenerateRelatedCareersDataSection()
        {
            return new RelatedCareersData()
            {
                Id = Guid.NewGuid().ToString(),
                ProfileLink = "/related-careers-link",
                Title = "This is the related careers title"
            };
        }
    }
}