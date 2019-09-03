using DFC.App.RelatedCareers.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.SegmentService
{
    public interface IRelatedCareersSegmentService
    {
        Task<IEnumerable<RelatedCareersSegmentModel>> GetAllAsync();

        Task<RelatedCareersSegmentModel> GetByIdAsync(Guid documentId);

        Task<RelatedCareersSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false);
    }
}