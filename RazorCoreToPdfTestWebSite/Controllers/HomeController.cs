using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RazorCoreToPdfTestWebSite.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorCoreToPdf;

namespace RazorCoreToPdfTestWebSite.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> PdfTestView() {
            return await this.RazorToPdf(DateTime.Now);
        }
    }
}