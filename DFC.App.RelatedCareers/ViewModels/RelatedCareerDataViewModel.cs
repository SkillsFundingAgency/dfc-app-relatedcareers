using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.ViewModels
{
    public class RelatedCareerDataViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string ProfileLink { get; set; }

        [Required]
        public string Title { get; set; }
    }
}