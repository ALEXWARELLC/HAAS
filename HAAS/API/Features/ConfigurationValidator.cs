using Exiled.API.Features;

namespace HAAS.API.Features;

internal class ConfigurationValidator
{
    public static bool IsWebhook(string Webhook)
    {
        if (string.IsNullOrEmpty(Webhook) || string.IsNullOrWhiteSpace(Webhook))
            return false;

        if (!Webhook.StartsWith("https://discord.com/api/webhooks/"))
            return false;

        return true;
    }

    public static bool ValidSensitivity(short Sensitivity)
    {
        if (Sensitivity < 0 || Sensitivity > Server.MaxPlayerCount)
            return false;

        return true;
    }
}
