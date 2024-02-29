using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace HAAS;

public class Plugin : Plugin<Config>
{
    public override string Name => "High Activity Alert System";
    public override string Author => "Cypher The Protogen";
    public override string Prefix => "HAAS";
    public override Version Version => new Version(0, 0, 1, 2);
    private API.Features.EventHandler? Events;
    
    public async override void OnEnabled()
    {
        Log.Info("Validating HAAS Configuration...");
        ValidateConfigs(Config);
        Log.Info("Configuring HAAS Event Handlers...");
        Events = new();
        Events.Enable(Config);

        if (Config.Debug)
            API.Features.WebhookConnector.Send(Config.WebhookURL, API.Features.WebhookConnector.BuildMessage("HAAS has been initialized successfully."));
    }

    private bool ValidateConfigs(Config config)
    {
        if (!API.Features.ConfigurationValidator.IsWebhook(config.WebhookURL))
        {
            Log.Error("The provided webhook is invalid. Please ensure you have set a valid webhook URL.");
            return false;
        }

        if (API.Features.ConfigurationValidator.ValidSensitivity(config.PlayerSensitivity))
        {
            Log.Error("You have provided an invalid Player Sensitivity. Please check your HAAS configuration.");
        }

        Log.Info("Your HAAS Configuration has been successfully validated.");
        return true;
    }

    public override void OnDisabled()
    {
        Log.Info("Shutting down HAAS...");
        Events?.Disable();
    }
}

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
    public string WebhookURL { get; set; } = string.Empty;
    public short PlayerSensitivity { get; set; } = 10;
    public string CustomMessage { get; set; } = "Join the server! There's lots of people.";
}
