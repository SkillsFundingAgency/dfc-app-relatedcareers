using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.Data.Models
{
    public class RelatedCareerDataModel
    {
        public Guid Id { get; set; }

        [Required]
        public string ProfileLink { get; set; }

        [Required]
        public string Title { get; set; }
    }
}