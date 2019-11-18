using System;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.Data.Models
{
    public class RelatedCareerSegmentDataModel
    {
        public const string SegmentName = "RelatedCareers";

        public DateTime LastReviewed { get; set; }

        public IEnumerable<RelatedCareerDataModel> RelatedCareers { get; set; }
    }
}