using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.Common.AzureServiceBusSupport
{
    public class Topic
    {
        private TopicClient TopicClient { get; set; }

        public Topic(string endpoint)
        {
            ServiceBusConnectionStringBuilder connectionString = new ServiceBusConnectionStringBuilder(endpoint);
            TopicClient = new TopicClient(connectionString);
        }

        public async Task SendAsync(Message message)
        {
            await TopicClient.SendAsync(message);
        }
    }
}
