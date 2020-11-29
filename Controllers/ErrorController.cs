using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace AspCoreFrame.WebUI.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly IStringLocalizer<ErrorController> _localizer;

        public ErrorController(IStringLocalizer<ErrorController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult UploadTooLarge()
        {
            TempData["Error"] = _localizer["FileTooLarge"];
            return View("Error");
        }

        [HttpGet]
        public IActionResult PageNotFound()
        {
            TempData["Error"] = _localizer["PageNotFound"];
            return View("Error");
        }
    }
}
