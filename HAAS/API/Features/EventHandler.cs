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
    }

    public void Disable()
    {
        Configuration = null;
        Exiled.Events.Handlers.Player.Joined -= OnPlayerJoin;
        ActivatedDuringRound = false;
    }

    private void OnPlayerJoin(JoinedEventArgs ev)
    {
        if (!ActivatedDuringRound)
        {
            if (Server.PlayerCount >= Configuration?.PlayerSensitivity)
            {
                WebhookConnector.Send(Configuration.WebhookURL, WebhookConnector.BuildMessage(Configuration.CustomMessage));
                ActivatedDuringRound = true;
            }
        }
    }
}
