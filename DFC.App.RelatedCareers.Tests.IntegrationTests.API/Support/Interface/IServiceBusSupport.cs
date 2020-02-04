using DFC.App.RelatedCareers.Tests.Common.AzureServiceBusSupport;
using System;
using System.Threading.Tasks;
using static DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.EnumLibrary;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    interface IServiceBusSupport
    {
        Message CreateServiceBusMessage(Guid messageId, byte[] messageBody, ContentType contentType, ActionType actionType, CType ctype);
        Message CreateServiceBusMessage(string messageId, byte[] messageBody, ContentType contentType, ActionType actionType, CType ctype);
        Task SendMessage(Topic topic, Message message);
    }
}
