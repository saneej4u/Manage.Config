using Manage.Configuration.Services.Configuration;
using Manage.Configuration.Service.Models;
using Manage.Config.Service.Models;
using Manage.Config.Service.Services.Mappers;
using Manage.Config.Service.Services.FileManager;

namespace Manage.Config.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationMapper _configurationMapper;
        private readonly IFileManagerService _fileManagerService;
        public ConfigurationService(IConfigurationMapper configurationMapper, IFileManagerService fileManagerService)
        {
            _configurationMapper = configurationMapper;
            _fileManagerService = fileManagerService;
        }
        public async Task<ResponseWrapper<ServerModel>> GetAllServerConfigurationAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return ResponseWrapper.CreateClientError("File path not exist.");
            }

            // Read file and group by empty line
            var configurations = await _fileManagerService.ReadFileAndGroupByEmptyLineAsync(filePath);

            if (configurations.Count == 0)
            {
                return ResponseWrapper.CreateNotFoundError("Server configuration is missing.");
            }

            // Find default server - first from the list
            var getDefaultConfiguration = configurations.FirstOrDefault();
            var defaultServer = _configurationMapper.MapToDefaultServerDetailsModel(getDefaultConfiguration);

            if (defaultServer is null || defaultServer.ServerType != ServerTypes.DEFAULT)
            {
                return ResponseWrapper.CreateNotFoundError("Default server not exist.");
            }

            // Get all specific servers - skip first and take rest
            var getSpecificServers = configurations.Skip(1);
            var specificServers = getSpecificServers.Select(serverDetails => _configurationMapper.MapToSpecificServerDetailsModel(serverDetails, defaultServer)).ToList();

            if (specificServers.Count == 0 || !specificServers.All(x => x.ServerType == ServerTypes.SERVER_SPECIFIC))
            {
                return ResponseWrapper.CreateNotFoundError("Specific servers not exist.");
            }

            var serverDetailsModel = new ServerModel
            {
                FilePath = filePath,
                DefaultServer = defaultServer,
                SpecificServers = specificServers
            };

            return ResponseWrapper.CreateSuccess(serverDetailsModel);
        }
    }
}
