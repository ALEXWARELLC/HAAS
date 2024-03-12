namespace HAAS.API.Interfaces;

internal interface IWebhook
{
    string Target { get; }
    string Name { get; }
    string Color { get; }
}
