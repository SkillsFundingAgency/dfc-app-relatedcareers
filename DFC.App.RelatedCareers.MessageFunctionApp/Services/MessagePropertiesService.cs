using Microsoft.Azure.ServiceBus;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public class MessagePropertiesService : IMessagePropertiesService
    {
        public long GetSequenceNumber(Message message)
        {
            return (message.SystemProperties?.SequenceNumber).GetValueOrDefault();
        }
    }
}