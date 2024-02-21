using Manage.Config.Service.Extensions;
using Manage.Config.Service.Models;

namespace Manage.Config.Service.Services.Mappers
{
    public class ConfigurationMapper : IConfigurationMapper
    {
        private const string SERVER_NAME = "SERVER_NAME";
        private const string URL = "URL";
        private const string DB = "DB";
        private const string IP_ADDRESS = "IP_ADDRESS";
        private const string DOMAIN = "DOMAIN";
        private const string COOKIE_DOMAIN = "COOKIE_DOMAIN";

        public ConfigurationMapper() { }
        public ServerDetailsModel MapToDefaultServerDetailsModel(List<string> serverDetails)
        {
            var defaultServer = new ServerDetailsModel
            {
                ServerName = serverDetails.FirstOrDefault(x => x.StartsWith(SERVER_NAME)).ToConfigurationItem(),
                URL = serverDetails.FirstOrDefault(x => x.StartsWith(URL)).ToConfigurationItem(),
                Database = serverDetails.FirstOrDefault(x => x.StartsWith(DB)).ToConfigurationItem(),
                IPAddress = serverDetails.FirstOrDefault(x => x.StartsWith(IP_ADDRESS)).ToConfigurationItem(),
                Domain = serverDetails.FirstOrDefault(x => x.StartsWith(DOMAIN)).ToConfigurationItem(),
                CookieDomain = serverDetails.FirstOrDefault(x => x.StartsWith(COOKIE_DOMAIN)).ToConfigurationItem()
            };

            defaultServer.ServerType = (string.IsNullOrEmpty(defaultServer.ServerName.ServerSuffixName) && defaultServer.ServerName.IsDefaultUsed) 
                                        ? Configuration.Service.Models.ServerTypes.DEFAULT 
                                        : Configuration.Service.Models.ServerTypes.SERVER_SPECIFIC;
            
            return defaultServer;
        }

        public ServerDetailsModel MapToSpecificServerDetailsModel(List<string> serverDetails, ServerDetailsModel defaultServer)
        {
            var serverSpecific = new ServerDetailsModel
            {
                ServerType = Configuration.Service.Models.ServerTypes.SERVER_SPECIFIC,
                ServerName = serverDetails.FirstOrDefault(x => x.StartsWith(SERVER_NAME)).ToConfigurationItem() ?? defaultServer.ServerName,
                URL = serverDetails.FirstOrDefault(x => x.StartsWith(URL)).ToConfigurationItem() ?? defaultServer.URL,
                Database = serverDetails.FirstOrDefault(x => x.StartsWith(DB)).ToConfigurationItem() ?? defaultServer.Database,
                IPAddress = serverDetails.FirstOrDefault(x => x.StartsWith(IP_ADDRESS)).ToConfigurationItem() ?? defaultServer.IPAddress,
                Domain = serverDetails.FirstOrDefault(x => x.StartsWith(DOMAIN)).ToConfigurationItem() ?? defaultServer.Domain,
                CookieDomain = serverDetails.FirstOrDefault(x => x.StartsWith(COOKIE_DOMAIN)).ToConfigurationItem() ?? defaultServer.CookieDomain
            };

            return serverSpecific;
        }
    }
}
