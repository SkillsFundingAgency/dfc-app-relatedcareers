using DFC.App.RelatedCareers.Data.Models;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public interface IMappingService
    {
        RelatedCareersSegmentModel MapToSegmentModel(string message, long sequenceNumber);
    }
}