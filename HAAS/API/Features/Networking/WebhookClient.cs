using Exiled.API.Features;
using HAAS.API.Interfaces;
using Newtonsoft.Json;
using System;
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

    // Until I figure out how to use HttpClient properly, I'll use WebRequest instead.
    //
    //public async Task SendMessageAsync(string Content)
    //{
    //    using (var client = new HttpClient())
    //    {
    //        await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, Target)
    //        {
    //            Content = new StringContent(JsonConvert.SerializeObject(new
    //            {
    //                username = Name,
    //                embeds = new[]
    //                {
    //                    new
    //                    {
    //                        title = "High Activity Alert System - ALPHA",
    //                        description = Content,
    //                        color = Color
    //                    }
    //                }
    //            }))
    //        });
    //    }
    //}

    // This is stupid and should be repalced as soon as Northwood releases the Surface Update.
    public async Task SendMessage(string Content)
    {
        WebRequest wr = (HttpWebRequest)WebRequest.Create(Target);
        wr.ContentType = "application/json";
        wr.Method = "POST";

        using (var streamWriter = new StreamWriter(wr.GetRequestStream()))
        {
            string json = JsonConvert.SerializeObject(new
            {
                username = Name,
                embeds = new[]
                {
                    new
                    {
                        title = $"High Activity Alert System - ALPHA",
                        description = Content,
                        color = Color
                    }
                }
            });
            streamWriter.Write(json);
        }

        var response = (HttpWebResponse)wr.GetResponse();

        if (response.StatusCode == HttpStatusCode.OK)
            return;
        else
            Log.Debug("Embed sent successfully.");
    }

    public void Dispose() { }
}
