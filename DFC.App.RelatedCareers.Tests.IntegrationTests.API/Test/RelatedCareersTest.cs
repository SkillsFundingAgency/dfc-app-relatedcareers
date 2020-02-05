using DFC.App.RelatedCareers.Tests.Common.APISupport;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Test
{
    public class RelatedCareersTest : SetUpAndTearDown
    {
        [Test]
        [Description("Tests that the CType 'JobProfile' successfully tiggers a related careers update to an existing job profile")]
        public async Task RelatedCareers_JobProfile()
        {
            Response<List<RelatedCareersAPIResponse>> response = await CommonAction.ExecuteGetRequest<List<RelatedCareersAPIResponse>>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId), GetRequest.ContentType.Json);
            Assert.AreEqual(JobProfile.RelatedCareersData.Count, response.Data.Count);
            Assert.AreEqual(JobProfile.RelatedCareersData[0].Title, response.Data[0].title);
            Assert.AreEqual(JobProfile.RelatedCareersData[0].ProfileLink, response.Data[0].url);
        }
    }
}