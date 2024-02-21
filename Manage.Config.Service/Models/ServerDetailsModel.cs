using Manage.Configuration.Service.Models;

namespace Manage.Config.Service.Models
{
    public class ServerDetailsModel
    {
        public ConfigurationItem ServerName { get; set; }
        public ConfigurationItem URL { get; set; }
        public ConfigurationItem Database { get; set; }
        public ConfigurationItem IPAddress { get; set; }
        public ConfigurationItem Domain { get; set; }
        public ConfigurationItem CookieDomain { get; set; }
        public ServerTypes ServerType { get; set; }
    }
}
