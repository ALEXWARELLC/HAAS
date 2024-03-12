using HAAS.API.Interfaces;

namespace HAAS.API;

public class WebhookSettings : IWebhook
{
    public string Target { get; set; } = "YOUR_WEBHOOK_HERE";

    public string Name { get; set; } = "High Activity Alert System";

    public string Color { get; set; } = "5763719";
}