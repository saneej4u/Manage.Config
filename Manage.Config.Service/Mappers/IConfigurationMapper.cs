using Manage.Config.Service.Models;
using Manage.Configuration.Service.Models;

namespace Manage.Config.Service.Services.Mappers
{
    public interface IConfigurationMapper
    {
        ServerDetailsModel MapToDefaultServerDetailsModel(List<string> serverDetails);
        ServerDetailsModel MapToSpecificServerDetailsModel(List<string> serverDetails, ServerDetailsModel defaultServer);
    }
}
