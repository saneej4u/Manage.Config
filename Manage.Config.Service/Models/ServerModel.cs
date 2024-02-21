using Manage.Config.Service.Models;

namespace Manage.Configuration.Service.Models
{
    public class ServerModel
    {
        public string FilePath { get; set; }
        public ServerDetailsModel DefaultServer { get; set; }
        public List<ServerDetailsModel> SpecificServers { get; set; }
    }

    public enum ServerTypes
    {
        DEFAULT,
        SERVER_SPECIFIC
    }
}
