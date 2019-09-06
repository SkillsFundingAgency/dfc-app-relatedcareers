using System;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.ViewModels
{
    public class DocumentDataViewModel
    {
        public IEnumerable<RelatedCareerDataViewModel> RelatedCareers { get; set; }

        public DateTime LastReviewed { get; set; }
    }
}