using Exiled.API.Features;
using Exiled.API.Interfaces;
using HAAS.API;
using HAAS.API.Features.Networking;
using HAAS.API.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace HAAS;

public class Plugin : Plugin<Config>
{
    public override string Name => "High Activity Alert System";
    public override string Author => "Cypher The Protogen";
    public override string Prefix => "haas";
    public override Version Version => new Version(0, 0, 1, 3);
    internal WebhookClient WebhookClient { get; private set; }
    internal PlayerEventHandler PlayerEvents { get; private set; }

    public override void OnEnabled()
    {
        WebhookClient = new WebhookClient(Config.Settings.WebhookSettings);
        PlayerEvents = new PlayerEventHandler(WebhookClient, Config.Settings);

        Exiled.Events.Handlers.Player.Verified += PlayerEvents.OnPlayerVerified;
        Exiled.Events.Handlers.Player.Left += PlayerEvents.OnPlayerLeft;
        Exiled.Events.Handlers.Server.EndingRound += PlayerEvents.OnRoundRestart;

        Log.Debug($"Set Minimum Player Count to: {Config.Settings.MinPlayerCount}");
    }

    public override void OnDisabled()
    {
        Exiled.Events.Handlers.Player.Verified -= PlayerEvents.OnPlayerVerified;
        Exiled.Events.Handlers.Player.Left -= PlayerEvents.OnPlayerLeft;
        Exiled.Events.Handlers.Server.EndingRound -= PlayerEvents.OnRoundRestart;
        
        WebhookClient.Dispose();
        PlayerEvents = null;
    }
}

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
    public Settings Settings { get; set; } = new();
}