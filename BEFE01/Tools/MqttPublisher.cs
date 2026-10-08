using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

public class MqttPublisher : IAsyncDisposable
{
    private readonly IMqttClient _client;

    public MqttPublisher()
    {
        _client = new MqttFactory().CreateMqttClient();
    }

    public async Task ConnectAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .WithClientId("BooksApi")
            .Build();

        await _client.ConnectAsync(options);
    }

    public async Task PublishAsync<T>(T item)
    {
        var json = JsonSerializer.Serialize(item);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic("books/created")
            .WithPayload(json)
            .WithRetainFlag(false)
            .Build();

        await _client.PublishAsync(message);
    }

    public async ValueTask DisposeAsync()
    {
        if (_client.IsConnected)
            await _client.DisconnectAsync();

        _client.Dispose();
    }
}