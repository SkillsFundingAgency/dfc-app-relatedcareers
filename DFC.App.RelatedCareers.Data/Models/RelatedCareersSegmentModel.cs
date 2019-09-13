using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.Data.Models
{
    public class RelatedCareersSegmentModel : IDataModel
    {
        private int partitionKey;

        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Updated { get; set; }

        public int PartitionKey
        {
            get => Created.Second;
            set => partitionKey = value;
        }

        public RelatedCareerSegmentDataModel Data { get; set; }
    }
}