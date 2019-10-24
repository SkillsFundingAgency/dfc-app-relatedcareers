using System;

namespace DFC.App.RelatedCareers.Data.ServiceBusModels
{
    public class RelatedCareersServiceBusModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ProfileLink { get; set; }
    }
}