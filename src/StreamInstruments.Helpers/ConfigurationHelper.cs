using Microsoft.Extensions.Configuration;

namespace StreamInstruments.Helpers;

public static class ConfigurationHelper
{
    public static IConfiguration GetConfig()
    {
        var configurationBuilder = new ConfigurationBuilder();

        return configurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(FileConstants.AppSettingsFileName)
            .Build();
    }
}