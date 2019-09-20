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

namespace DFC.App.RelatedCareers.Controllers
{
    public class SegmentController : Controller
    {
        public const string SegmentRoutePrefix = "segment";
        public const string JobProfileRoutePrefix = "jobprofile";

        private const string IndexActionName = nameof(Index);
        private const string DocumentActionName = nameof(Document);
        private const string BodyActionName = nameof(Body);
        private const string SaveActionName = nameof(Save);
        private const string DeleteActionName = nameof(Delete);

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

        [HttpPut]
        [HttpPost]
        [Route("segment")]
        public async Task<IActionResult> Save([FromBody]RelatedCareersSegmentModel upsertRelatedCareersSegmentModel)
        {
            logger.LogInformation($"{SaveActionName} has been called");

            if (upsertRelatedCareersSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await relatedCareersSegmentService.UpsertAsync(upsertRelatedCareersSegmentModel)
                .ConfigureAwait(false);

            if (response.ResponseStatusCode == HttpStatusCode.Created)
            {
                logger.LogInformation($"{SaveActionName} has created content for: {upsertRelatedCareersSegmentModel.CanonicalName}");

                return new CreatedAtActionResult(
                    SaveActionName,
                    "Segment",
                    new { article = response.RelatedCareersSegmentModel.CanonicalName },
                    response.RelatedCareersSegmentModel);
            }
            else
            {
                logger.LogInformation($"{SaveActionName} has updated content for: {upsertRelatedCareersSegmentModel.CanonicalName}");

                return new OkObjectResult(response.RelatedCareersSegmentModel);
            }
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
