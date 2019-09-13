using System.Net;

namespace DFC.App.RelatedCareers.Data.Models
{
    public class UpsertRelatedCareersSegmentModelResponse
    {
        public RelatedCareersSegmentModel RelatedCareersSegmentModel { get; set; }

        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}