﻿using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using HAAS.API;
using HAAS.API.Features.Networking;

namespace HAAS;

internal class PlayerEventHandler
{
    internal bool PreTriggered { get; private set; } = false;
    internal ushort PlayerCount { get; private set; } = 0;
    internal WebhookClient WebhookClient { get; }
    internal Settings LocalSettings { get; }
    internal PlayerEventHandler(WebhookClient WebhookClient, Settings? Settings)
    {
        this.WebhookClient = WebhookClient;
        LocalSettings = Settings ?? new Settings();
    }

    internal void OnRoundRestart(EndingRoundEventArgs _)
    {
        PreTriggered = !LocalSettings.ResetOnRestart;
        PlayerCount = 0;
    }

    internal void OnPlayerLeft(LeftEventArgs _)
    {
        if (!_.Player.IsNPC)
            PlayerCount--;
    }

    internal void OnPlayerVerified(VerifiedEventArgs _)
    {
        if (!_.Player.IsNPC)
            PlayerCount++;

        Task.Run(async () => await SendAlert());
    }

    public async Task SendAlert()
    {
        if (PreTriggered || PlayerCount < LocalSettings.MinPlayerCount)
            return;

        if (await WebhookClient.SendMessageAsync(LocalSettings.WebhookMessage))
        {
            // The Webhook managed to be sent, so we can set the PreTriggered flag to true.
            PreTriggered = true;
        };

        // If the Webhook failed to send, we don't set the PreTriggered flag to true
        // and it'll retry sending messages to the webhook on the next player verification.
    }
}
