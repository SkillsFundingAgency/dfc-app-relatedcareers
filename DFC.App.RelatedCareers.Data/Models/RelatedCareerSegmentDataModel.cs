using System;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.Data.Models
{
    public class RelatedCareerSegmentDataModel
    {
        public IEnumerable<RelatedCareerDataModel> RelatedCareers { get; set; }

        public DateTime LastReviewed { get; set; }
    }
}