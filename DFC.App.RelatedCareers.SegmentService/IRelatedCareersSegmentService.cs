using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.Models.PatchModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.SegmentService
{
    public interface IRelatedCareersSegmentService
    {
        Task<bool> PingAsync();

        Task<IEnumerable<RelatedCareersSegmentModel>> GetAllAsync();

        Task<RelatedCareersSegmentModel> GetByIdAsync(Guid documentId);

        Task<RelatedCareersSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false);

        Task<HttpStatusCode> UpsertAsync(RelatedCareersSegmentModel relatedCareersSegmentModel);

        Task<HttpStatusCode> PatchRelatedCareerAsync(PatchRelatedCareersDataModel patchModel, Guid documentId);

        Task<bool> DeleteAsync(Guid documentId);
    }
}