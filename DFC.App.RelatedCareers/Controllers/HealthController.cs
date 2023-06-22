using DFC.App.RelatedCareers.Extensions;
using DFC.App.RelatedCareers.SegmentService;
using DFC.App.RelatedCareers.ViewModels;
using DFC.Logger.AppInsights.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Controllers
{
    public class HealthController : Controller
    {
        private const string SuccessMessage = "Document store is available";

        private readonly ILogService logService;
        private readonly IRelatedCareersSegmentService relatedCareersSegmentService;

        private readonly string resourceName;

        public HealthController(ILogService logService, IRelatedCareersSegmentService relatedCareersSegmentService)
        {
            this.logService = logService;
            this.relatedCareersSegmentService = relatedCareersSegmentService;

            resourceName = typeof(Program).Namespace;
        }

        [HttpGet]
        public IActionResult Ping()
        {
            logService.LogInformation($"{nameof(Ping)} has been called");

            return Ok();
        }

        [HttpGet]
        [Route("health")]
        public async Task<IActionResult> Health()
        {
            logService.LogInformation($"{nameof(Health)} has been called");

            try
            {
                var isHealthy = await relatedCareersSegmentService.PingAsync().ConfigureAwait(false);
                if (isHealthy)
                {
                    logService.LogInformation($"{nameof(Health)} responded with: {resourceName} - {SuccessMessage}");

                    var viewModel = CreateHealthViewModel();

                    return this.NegotiateContentResult(viewModel);
                }

                logService.LogError($"{nameof(Health)}: Ping to {resourceName} has failed");
            }
            catch (Exception ex)
            {
                logService.LogError($"{nameof(Health)}: {resourceName} exception: {ex.Message}");
            }

            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        private HealthViewModel CreateHealthViewModel()
        {
            logService.LogInformation($"{nameof(CreateHealthViewModel)} has been called");

            return new HealthViewModel
            {
                HealthItems = new List<HealthItemViewModel>
                {
                    new HealthItemViewModel
                    {
                        Service = resourceName,
                        Message = SuccessMessage,
                    },
                },
            };
        }
    }
}