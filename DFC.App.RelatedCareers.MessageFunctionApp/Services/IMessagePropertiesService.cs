using Microsoft.Azure.ServiceBus;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public interface IMessagePropertiesService
    {
        long GetSequenceNumber(Message message);
    }
}