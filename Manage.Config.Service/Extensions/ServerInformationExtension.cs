using Manage.Config.Service.Models;

namespace Manage.Config.Service.Extensions
{
    public static class ServerInformationExtension
    {
        public static ConfigurationItem ToConfigurationItem(this string serverItem)
        {
            if (serverItem == null) return null;

            string[] serverItemParts = serverItem.Split('=');
            if (serverItemParts.Length >= 2)
            {
                string key = serverItemParts[0];
                string value = string.Join("=", serverItemParts.Skip(1));

                string suffixValue = null;
                if (key.Contains('{') && key.Contains('}'))
                {
                    int startIndex = key.IndexOf('{') + 1;
                    int endIndex = key.IndexOf('}');
                    suffixValue = key.Substring(startIndex, endIndex - startIndex);
                    key = key.Substring(0, startIndex - 1);
                }

                var configItem = new ConfigurationItem
                {
                    Key = key,
                    Value = value.Trim(),
                    ServerSuffixName = suffixValue ?? string.Empty,
                    IsDefaultUsed = suffixValue == null
                };

                return configItem;
            }

            return null;
        }
    }
}
