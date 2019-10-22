using System;

namespace DFC.App.RelatedCareers.Data.Models.PatchModels
{
    public class PatchRelatedCareersDataModel : BasePatchModel
    {
        public string CanonicalName { get; set; }

        public string Title { get; set; }

        public string SocLevelTwo { get; set; }
    }
}