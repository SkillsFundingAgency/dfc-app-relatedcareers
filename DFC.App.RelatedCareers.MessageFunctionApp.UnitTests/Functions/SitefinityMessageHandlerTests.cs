using DFC.App.RelatedCareers.Data.Enums;
using DFC.App.RelatedCareers.MessageFunctionApp.Functions;
using DFC.App.RelatedCareers.MessageFunctionApp.Services;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.RelatedCareers.MessageFunctionApp.UnitTests.Functions
{
    public class SitefinityMessageHandlerTests
    {
        public static IEnumerable<object[]> ValidStatusCodes => new List<object[]>
        {
            new object[] { HttpStatusCode.OK },
            new object[] { HttpStatusCode.Created },
            new object[] { HttpStatusCode.AlreadyReported },
        };

        [Fact]
        public async Task RunHandlerThrowsArgumentNullExceptionWhenMessageIsNull()
        {
            // Arrange
            var processor = A.Fake<IMessageProcessor>();
            var messagePropertiesService = A.Fake<IMessagePropertiesService>();
            var logService = A.Fake<ILogService>();
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            var sitefinityMessageHandler = new SitefinityMessageHandler(processor, messagePropertiesService, logService, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await sitefinityMessageHandler.Run(null).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerThrowsArgumentExceptionWhenMessageBodyIsEmpty()
        {
            // Arrange
            var message = new Message(Encoding.ASCII.GetBytes(string.Empty));
            var processor = A.Fake<IMessageProcessor>();
            var messagePropertiesService = A.Fake<IMessagePropertiesService>();
            var logService = A.Fake<ILogService>();
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            var sitefinityMessageHandler = new SitefinityMessageHandler(processor, messagePropertiesService, logService, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<ArgumentException>(async () => await sitefinityMessageHandler.Run(message).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerThrowsArgumentOutOfRangeExceptionWhenMessageActionIsInvalid()
        {
            // Arrange
            var message = CreateBaseMessage(messageAction: (MessageAction)999);
            var processor = A.Fake<IMessageProcessor>();
            var messagePropertiesService = A.Fake<IMessagePropertiesService>();
            var logService = A.Fake<ILogService>();
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            var sitefinityMessageHandler = new SitefinityMessageHandler(processor, messagePropertiesService, logService, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sitefinityMessageHandler.Run(message).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerThrowsArgumentOutOfRangeExceptionWhenMessageContentTypeIsInvalid()
        {
            // Arrange
            var message = CreateBaseMessage(contentType: (MessageContentType)999);
            var processor = A.Fake<IMessageProcessor>();
            var messagePropertiesService = A.Fake<IMessagePropertiesService>();
            var logService = A.Fake<ILogService>();
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            var sitefinityMessageHandler = new SitefinityMessageHandler(processor, messagePropertiesService, logService, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sitefinityMessageHandler.Run(message).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private Message CreateBaseMessage(MessageAction messageAction = MessageAction.Published, MessageContentType contentType = MessageContentType.JobProfile)
        {
            var message = A.Fake<Message>();
            message.Body = Encoding.ASCII.GetBytes("Some body json object here");
            message.UserProperties.Add("ActionType", messageAction.ToString());
            message.UserProperties.Add("CType", contentType);

            return message;
        }
    }
}