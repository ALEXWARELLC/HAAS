namespace HAAS.API;

public class Settings
{
    public bool ResetOnRestart { get; set; } = false;
    public ushort MinPlayerCount { get; set; } = 10;
    public string WebhookMessage { get; set; } = "HAAS has detected high player activity on your server. Your server is popular right now!";
    public WebhookSettings WebhookSettings { get; set; } = new WebhookSettings();
}
