using AutoMapper;
using DFC.App.RelatedCareers.Data.Enums;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.Models.PatchModels;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.DraftSegmentService;
using DFC.App.RelatedCareers.Repository.CosmosDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.SegmentService
{
    public class RelatedCareersSegmentService : IRelatedCareersSegmentService
    {
        private readonly ICosmosRepository<RelatedCareersSegmentModel> repository;
        private readonly IDraftRelatedCareersSegmentService draftRelatedCareersSegmentService;
        private readonly IMapper mapper;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;

        public RelatedCareersSegmentService(ICosmosRepository<RelatedCareersSegmentModel> repository, IDraftRelatedCareersSegmentService draftRelatedCareersSegmentService, IMapper mapper, IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService)
        {
            this.repository = repository;
            this.draftRelatedCareersSegmentService = draftRelatedCareersSegmentService;
            this.mapper = mapper;
            this.jobProfileSegmentRefreshService = jobProfileSegmentRefreshService;
        }

        public async Task<bool> PingAsync()
        {
            return await repository.PingAsync().ConfigureAwait(false);
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

        public async Task<HttpStatusCode> UpsertAsync(RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            if (relatedCareersSegmentModel == null)
            {
                throw new ArgumentNullException(nameof(relatedCareersSegmentModel));
            }

            if (relatedCareersSegmentModel.Data == null)
            {
                relatedCareersSegmentModel.Data = new RelatedCareerSegmentDataModel();
            }

            return await UpsertAndRefreshSegmentModel(relatedCareersSegmentModel).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> PatchRelatedCareerAsync(PatchRelatedCareersDataModel patchModel, Guid documentId)
        {
            if (patchModel is null)
            {
                throw new ArgumentNullException(nameof(patchModel));
            }

            var existingSegmentModel = await GetByIdAsync(documentId).ConfigureAwait(false);
            if (existingSegmentModel is null)
            {
                return HttpStatusCode.NotFound;
            }

            if (patchModel.SequenceNumber <= existingSegmentModel.SequenceNumber)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var existingRelatedCareer = existingSegmentModel.Data.RelatedCareers.SingleOrDefault(r => r.Id == patchModel.Id);
            if (existingRelatedCareer is null)
            {
                return patchModel.MessageAction == MessageAction.Deleted ? HttpStatusCode.AlreadyReported : HttpStatusCode.NotFound;
            }

            var filteredRelatedCareers = existingSegmentModel.Data.RelatedCareers.Where(ai => ai.Id != patchModel.Id).ToList();

            if (patchModel.MessageAction == MessageAction.Published || patchModel.MessageAction == MessageAction.Draft)
            {
                var updatedCareer = mapper.Map<RelatedCareerDataModel>(patchModel);
                var existingIndex = existingSegmentModel.Data.RelatedCareers.ToList().FindIndex(ai => ai.Id == patchModel.Id);
                filteredRelatedCareers.Insert(existingIndex, updatedCareer);
            }

            existingSegmentModel.Data.RelatedCareers = filteredRelatedCareers;
            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;

            return await UpsertAndRefreshSegmentModel(existingSegmentModel).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }

        private async Task<HttpStatusCode> UpsertAndRefreshSegmentModel(RelatedCareersSegmentModel existingSegmentModel)
        {
            var result = await repository.UpsertAsync(existingSegmentModel).ConfigureAwait(false);

            if (result == HttpStatusCode.OK || result == HttpStatusCode.Created)
            {
                var refreshJobProfileSegmentServiceBusModel = mapper.Map<RefreshJobProfileSegmentServiceBusModel>(existingSegmentModel);

                await jobProfileSegmentRefreshService.SendMessageAsync(refreshJobProfileSegmentServiceBusModel).ConfigureAwait(false);
            }

            return result;
        }
    }
}