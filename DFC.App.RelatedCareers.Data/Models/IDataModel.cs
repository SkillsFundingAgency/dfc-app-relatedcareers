using Newtonsoft.Json;
using System;

namespace DFC.App.RelatedCareers.Data.Models
{
    public interface IDataModel
    {
        [JsonProperty(PropertyName = "id")]
        Guid DocumentId { get; set; }

        int PartitionKey { get; set; }
    }
}