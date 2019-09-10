using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.ViewModels
{
    public class RelatedCareerDataViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime Updated { get; set; }
    }
}