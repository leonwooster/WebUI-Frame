using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Localization;

using AspCoreFrame.WebUI.Models;
using AspCoreFrame.Services;

using AutoMapper;

namespace AspCoreFrame.WebUI.Controllers
{
    /// <summary>
    /// A class that handles the CRUD operations.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonDataService _commonService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<HomeController> _localizer;

        /// <summary>
        /// A constructor that receives multiple DI components.
        /// </summary>
        /// <param name="logger">File logger component.</param>
        /// <param name="commonService">Common data service component.</param>
        /// <param name="mapper">AutoMapper component.</param>
        public HomeController(ILogger<HomeController> logger,                                
                                ICommonDataService commonService,
                                IMapper mapper,
                                IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _commonService = commonService;
            _mapper = mapper;
            _localizer = localizer;
        }

        /// <summary>        
        /// 
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns></returns>
        public IActionResult Index(long? id)
        {
            try
            {
                SetBaseUrl();

                ViewData["Title"] = _localizer["PageTitle"];

                FormModel m = new FormModel();                
                return View("Index", m);
            }
            catch (Exception ex)
            {
                TempData["Error"] = _localizer["SaveError"];
                _logger.LogError(ex, "Application Error");
                return View("Error");
            }
        }

        [Authorize(AuthenticationSchemes = AzureADDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Save(FormModel m)
        {
            try
            {
                SetBaseUrl();
                TempData["Success"] = "Form data is saved successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = _localizer["SaveError"];
                _logger.LogError(ex, "Application Error");
                return View("Error");
            }

            return RedirectToAction("Index", new { id = m.id });
        }

        #region Helpers
        private string GetMimeType(string fileName)
        {
            // Make Sure Microsoft.AspNetCore.StaticFiles Nuget Package is installed
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public string BasePath()
        {
            return $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        }

        private void SetBaseUrl()
        {
            ViewBag.baseurl = BasePath();
        }

        #endregion
    }
}