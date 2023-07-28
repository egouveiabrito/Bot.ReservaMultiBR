using Microsoft.Extensions.Configuration;
namespace AutomationTest.Core
{
    public class ConfigurationHelper
    {
        protected readonly IConfiguration _config;
        public ConfigurationHelper()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
        public string WebDrivers => string.IsNullOrWhiteSpace(_config.GetSection("DirWebDrivers").Value) ? $"{Directory.GetCurrentDirectory()}\\WebDrivers" : _config.GetSection("DirWebDrivers").Value;

        public bool IsBrowser => bool.Parse(_config.GetSection("IsBrowser").Value);
    }
}