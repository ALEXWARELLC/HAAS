using HAAS.API.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Security.Policy;

namespace HAAS.API.Features.Networking;

internal class WebhookClient : IDisposable
{
    internal readonly string Target = string.Empty;
    internal readonly string Name = string.Empty;
    internal readonly string Color = string.Empty;
    internal WebhookClient(IWebhook Webhook)
    {
        Target = Webhook.Target;
        Name = Webhook.Name;
        Color = FilterColor(Webhook.Color);
    }

    private string FilterColor(string color)
    {
        if (color.StartsWith("#"))
            color.Remove(0, 1);

        return color;
    }

    public async Task<bool> SendMessageAsync(string Content)
    {
        using (var client = new HttpClient())
        {
            HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, Target)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    username = Name,
                    embeds = new[]
                    {
                        new
                        {
                            name = "High Activity Alert System - ALPHA",
                            description = Content,
                            color = Color
                        }
                    }
                }))
            });

            return response.IsSuccessStatusCode;
        }
    }

    public void Dispose() { }
}
