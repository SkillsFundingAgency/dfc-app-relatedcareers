using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.Data.ServiceBusModels
{
    public class BaseJobProfileMessage
    {
        [Required]
        public Guid JobProfileId { get; set; }
    }
}