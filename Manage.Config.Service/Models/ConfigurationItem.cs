namespace Manage.Config.Service.Models
{
    public class ConfigurationItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsDefaultUsed { get; set; }
        public string ServerSuffixName { get; set; }
    }
}
