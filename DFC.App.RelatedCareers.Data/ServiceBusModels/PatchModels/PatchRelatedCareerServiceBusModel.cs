using System;

namespace DFC.App.RelatedCareers.Data.ServiceBusModels.PatchModels
{
    public class PatchRelatedCareerServiceBusModel : BaseJobProfileMessage
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ProfileLink { get; set; }
    }
}