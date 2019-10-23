using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.Data.ServiceBusModels
{
    public class JobProfileMessage : BaseJobProfileMessage
    {
        [Required]
        public string CanonicalName { get; set; }

        public DateTime LastModified { get; set; }

        [Required]
        public string SocLevelTwo { get; set; }

        public IEnumerable<RelatedCareersServiceBusModel> RelatedCareersData { get; set; }
    }
}