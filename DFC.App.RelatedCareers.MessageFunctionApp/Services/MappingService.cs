using AutoMapper;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using Newtonsoft.Json;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Services
{
    public class MappingService : IMappingService
    {
        private readonly IMapper mapper;

        public MappingService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public RelatedCareersSegmentModel MapToSegmentModel(string message, long sequenceNumber)
        {
            var fullJobProfileMessage = JsonConvert.DeserializeObject<JobProfileMessage>(message);
            var fullJobProfile = mapper.Map<RelatedCareersSegmentModel>(fullJobProfileMessage);
            fullJobProfile.SequenceNumber = sequenceNumber;

            return fullJobProfile;
        }
    }
}