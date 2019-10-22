using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Extensions;
using DFC.App.RelatedCareers.SegmentService;
using DFC.App.RelatedCareers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DFC.App.RelatedCareers.Data.Models.PatchModels;

namespace DFC.App.RelatedCareers.Controllers
{
    public class SegmentController : Controller
    {
        public const string SegmentRoutePrefix = "segment";
        public const string JobProfileRoutePrefix = "jobprofile";

        private const string IndexActionName = nameof(Index);
        private const string DocumentActionName = nameof(Document);
        private const string BodyActionName = nameof(Body);
        private const string PostActionName = nameof(Post);
        private const string PutActionName = nameof(Put);
        private const string DeleteActionName = nameof(Delete);
        private const string PatchRelatedCareersActionName = nameof(PatchRelatedCareersData);

        private readonly ILogger<SegmentController> logger;
        private readonly IRelatedCareersSegmentService relatedCareersSegmentService;
        private readonly AutoMapper.IMapper mapper;

        public SegmentController(ILogger<SegmentController> logger, IRelatedCareersSegmentService relatedCareersSegmentService, AutoMapper.IMapper mapper)
        {
            this.logger = logger;
            this.relatedCareersSegmentService = relatedCareersSegmentService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/")]
        [Route("segment")]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation($"{IndexActionName} has been called");

            var viewModel = new IndexViewModel();
            var relatedCareersSegmentModels = await relatedCareersSegmentService.GetAllAsync().ConfigureAwait(false);

            if (relatedCareersSegmentModels != null)
            {
                viewModel.Documents = (from a in relatedCareersSegmentModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                logger.LogInformation($"{IndexActionName} has succeeded");
            }
            else
            {
                logger.LogWarning($"{IndexActionName} has returned with no results");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("segment/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            logger.LogInformation($"{DocumentActionName} has been called with: {article}");

            var relatedCareersSegmentModel = await relatedCareersSegmentService.GetByNameAsync(article, Request.IsDraftRequest()).ConfigureAwait(false);
            if (relatedCareersSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(relatedCareersSegmentModel);

                viewModel.RoutePrefix = SegmentRoutePrefix;

                logger.LogInformation($"{DocumentActionName} has succeeded for: {article}");

                return View(viewModel);
            }

            logger.LogWarning($"{DocumentActionName} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("segment/{article}/contents")]
        public async Task<IActionResult> Body(string article)
        {
            logger.LogInformation($"{BodyActionName} has been called with: {article}");

            var relatedCareersSegmentModel = await relatedCareersSegmentService.GetByNameAsync(article, Request.IsDraftRequest()).ConfigureAwait(false);
            if (relatedCareersSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(relatedCareersSegmentModel);

                viewModel.RoutePrefix = JobProfileRoutePrefix;

                logger.LogInformation($"{BodyActionName} has succeeded for: {article}");

                return this.NegotiateContentResult(viewModel, relatedCareersSegmentModel.Data);
            }

            logger.LogWarning($"{BodyActionName} has returned no content for: {article}");

            return NoContent();
        }

        [HttpPost]
        [Route("segment")]
        public async Task<IActionResult> Post([FromBody]RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            logger.LogInformation($"{PostActionName} has been called");

            if (relatedCareersSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await relatedCareersSegmentService.GetByIdAsync(relatedCareersSegmentModel.DocumentId).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            var response = await relatedCareersSegmentService.UpsertAsync(relatedCareersSegmentModel).ConfigureAwait(false);

            logger.LogInformation($"{PostActionName} has upserted content for: {relatedCareersSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPut]
        [Route("segment")]
        public async Task<IActionResult> Put([FromBody]RelatedCareersSegmentModel relatedCareersSegmentModel)
        {
            logger.LogInformation($"{PostActionName} has been called");

            if (relatedCareersSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await relatedCareersSegmentService.GetByIdAsync(relatedCareersSegmentModel.DocumentId).ConfigureAwait(false);
            if (existingDocument == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (relatedCareersSegmentModel.SequenceNumber <= existingDocument.SequenceNumber)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            relatedCareersSegmentModel.Etag = existingDocument.Etag;
            relatedCareersSegmentModel.SocLevelTwo = existingDocument.SocLevelTwo;

            var response = await relatedCareersSegmentService.UpsertAsync(relatedCareersSegmentModel).ConfigureAwait(false);

            logger.LogInformation($"{PostActionName} has upserted content for: {relatedCareersSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPatch]
        [Route("segment/{documentId}/relatedCareersData")]
        public async Task<IActionResult> PatchRelatedCareersData([FromBody]PatchRelatedCareersDataModel patchRelatedCareersDataModel, Guid documentId)
        {
            logger.LogInformation($"{PatchRelatedCareersActionName} has been called");

            if (patchRelatedCareersDataModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await relatedCareersSegmentService.PatchRelatedCareerAsync(patchRelatedCareersDataModel, documentId).ConfigureAwait(false);
            if (response != HttpStatusCode.OK && response != HttpStatusCode.Created)
            {
                logger.LogError($"{PatchRelatedCareersActionName}: Error while patching Related Career content for Job Profile with Id: {patchRelatedCareersDataModel.JobProfileId} for the {patchRelatedCareersDataModel.Title} career");
            }

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("segment/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            logger.LogInformation($"{DeleteActionName} has been called");

            var isDeleted = await relatedCareersSegmentService.DeleteAsync(documentId).ConfigureAwait(false);
            if (isDeleted)
            {
                logger.LogInformation($"{DeleteActionName} has deleted content for document Id: {documentId}");
                return Ok();
            }
            else
            {
                logger.LogWarning($"{DeleteActionName} has returned no content for: {documentId}");
                return NotFound();
            }
        }
    }
}