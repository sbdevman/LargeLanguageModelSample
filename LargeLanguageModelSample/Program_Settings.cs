using LargeLanguageModelSample;
using Microsoft.Extensions.Configuration;
partial class Program 
{
    private static Settings? GetSetttings()
    {
        const string settingsFile = "appsettings.json";
        const string settingsSectionKey = nameof(Settings);

        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile(settingsFile)
            .AddEnvironmentVariables()
            .Build();

        Settings? settings = config.GetRequiredSection(settingsSectionKey)
            .Get<Settings>();

        if (settings is null)
        {
            Console.WriteLine($"{settingsSectionKey} section not found.");
            return null;
        }

        return settings;
    }
}