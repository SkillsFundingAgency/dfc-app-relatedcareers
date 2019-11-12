using System;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.ViewModels
{
    public class DocumentDataViewModel
    {
        public DateTime LastReviewed { get; set; }

        public IEnumerable<RelatedCareerDataViewModel> RelatedCareers { get; set; }
    }
}