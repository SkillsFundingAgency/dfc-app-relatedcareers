﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.RelatedCareers.ViewModels
{
    public class DocumentViewModel
    {
        [Display(Name = "Document Id")]
        public Guid? DocumentId { get; set; }

        [Display(Name = "Canonical Name")]
        public string CanonicalName { get; set; }

        public DateTime LastReviewed { get; set; }

        public string RoutePrefix { get; set; }

        public DocumentDataViewModel Data { get; set; }
    }
}