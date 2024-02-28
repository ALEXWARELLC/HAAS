using System.Net.Http;
using System.Net.Http.Json;

namespace HAAS.API.Features;

internal class WebhookConnector
{
    record WebhookContent(string Content);
    public static string BuildMessage(string Message)
    {
        return $"## High Activity Alert System \n\n {Message}";
    }

    public static async Task Send(string url, string message)
    {
        using (var client = new HttpClient())
        {
            await client.PostAsJsonAsync(url, new WebhookContent(message));
        }
    }
}