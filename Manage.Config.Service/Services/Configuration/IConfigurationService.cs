using Manage.Config.Service.Models;
using Manage.Configuration.Service.Models;

namespace Manage.Configuration.Services.Configuration
{
    public interface IConfigurationService
    {
        Task<ResponseWrapper<ServerModel>> GetAllServerConfigurationAsync(string fullPath);
    }
}
