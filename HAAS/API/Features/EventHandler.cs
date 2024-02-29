using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace HAAS.API.Features;

internal class EventHandler
{
    private Config? Configuration;

    private bool ActivatedDuringRound = false;
    public void Enable(Config _config)
    {
        Configuration = _config;
        Exiled.Events.Handlers.Player.Joined += OnPlayerJoin;
        Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
    }

    public void Disable()
    {
        Configuration = null;
        Exiled.Events.Handlers.Player.Joined -= OnPlayerJoin;
        Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
    }

    private void OnPlayerJoin(JoinedEventArgs ev)
    {
        if (!ActivatedDuringRound)
        {
            if (Server.PlayerCount >= Configuration?.PlayerSensitivity)
            {
                ActivatedDuringRound = true;
                WebhookConnector.Send(Configuration.WebhookURL, WebhookConnector.BuildMessage(Configuration.CustomMessage));
            }
        }
    }

    private void OnPlayerLeft(LeftEventArgs ev)
    {
        if (Server.PlayerCount >= Configuration?.PlayerSensitivity)
            ActivatedDuringRound = false;
    }
}
