using Manage.Config.Service.Models;
using Manage.Configuration.Services.Configuration;
using Microsoft.AspNetCore.Mvc;
using Manage.Config.API.Utils;

namespace Manage.Config.API.Controllers
{
    [ApiController]
    [Route("api/saneej/v1.0/configuration")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }


        [HttpGet]
        [Route("view")]
        public async Task<ActionResult<ResponseWrapper<bool>>> GetServerConfiguartionsAsync()
        {
            var result = await _configurationService.GetAllServerConfigurationAsync(Path.GetFullPath(@"ConfigFiles\config.txt"));
            return result.ToHttpResponse();
        }
    }
}
