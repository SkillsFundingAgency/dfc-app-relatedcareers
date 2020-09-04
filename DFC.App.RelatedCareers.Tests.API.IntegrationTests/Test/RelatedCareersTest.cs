using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API.RestFactory;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Test
{
    public class RelatedCareersTest : SetUpAndTearDown
    {
        private IRelatedCareersAPI relatedCareersApi;

        [SetUp]
        public void SetUp()
        {
            this.relatedCareersApi = new RelatedCareersAPI(new RestClientFactory(), new RestRequestFactory(), this.AppSettings);
        }

        [Test]
        public async Task RelatedCareersJobProfile()
        {
            var response = await this.relatedCareersApi.GetById(this.JobProfile.JobProfileId).ConfigureAwait(false);
            Assert.AreEqual(this.JobProfile.RelatedCareersData.Count, response.Data.Count);
            Assert.AreEqual(this.JobProfile.RelatedCareersData[0].Title, response.Data[0].title);
            Assert.AreEqual(this.JobProfile.RelatedCareersData[0].ProfileLink, response.Data[0].url);
        }
    }
}