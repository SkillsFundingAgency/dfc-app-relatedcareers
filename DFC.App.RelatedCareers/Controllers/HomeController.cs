﻿using DFC.App.RelatedCareers.ViewModels;
using DFC.Logger.AppInsights.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DFC.App.RelatedCareers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogService logService;

        public HomeController(ILogService logService)
        {
            this.logService = logService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            logService.LogInformation($"{nameof(Error)} has been called");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}