using Manage.Config.Web.Models;
using Manage.Configuration.Services.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Manage.Config.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfigurationService _configurationService;

        public HomeController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _configurationService.GetAllServerConfigurationAsync(Path.GetFullPath(@"ConfigFiles\config.txt"));
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}