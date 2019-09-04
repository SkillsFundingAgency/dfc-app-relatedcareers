using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.DraftSegmentService;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.SegmentService
{
    public class RelatedCareersSegmentService : IRelatedCareersSegmentService
    {
        private readonly ICosmosRepository<RelatedCareersSegmentModel> repository;
        private readonly IDraftRelatedCareersSegmentService draftRelatedCareersSegmentService;

        public RelatedCareersSegmentService(ICosmosRepository<RelatedCareersSegmentModel> repository, IDraftRelatedCareersSegmentService draftRelatedCareersSegmentService)
        {
            this.repository = repository;
            this.draftRelatedCareersSegmentService = draftRelatedCareersSegmentService;
        }

        public async Task<IEnumerable<RelatedCareersSegmentModel>> GetAllAsync()
        {
            return await repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<RelatedCareersSegmentModel> GetByIdAsync(Guid documentId)
        {
            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<RelatedCareersSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return isDraft
                ? await draftRelatedCareersSegmentService.GetSitefinityData(canonicalName.ToLowerInvariant()).ConfigureAwait(false)
                : await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }
    }
}