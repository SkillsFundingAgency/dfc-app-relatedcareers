using DFC.App.RelatedCareers.Tests.Common.AzureServiceBusSupport;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    interface IJobProfileSupport
    {
        JobProfileContentType GenerateJobProfileContentType();
        Task DeleteJobProfile(Topic topic, JobProfileContentType jobProfileId);
    }
}
