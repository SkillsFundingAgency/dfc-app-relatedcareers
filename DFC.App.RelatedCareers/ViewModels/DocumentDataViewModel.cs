using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.ViewModels
{
    public class DocumentDataViewModel
    {
        [Display(Name = "Last Updated")]
        public DateTime LastReviewed { get; set; }

        public IEnumerable<RelatedCareerDataViewModel> RelatedCareers { get; set; }
    }
}