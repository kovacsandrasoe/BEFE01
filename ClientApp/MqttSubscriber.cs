using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

public class MqttSubscriber<T> : IDisposable
{
    private readonly IMqttClient _client;

    public event Action<T>? OnItemReceived;

    public MqttSubscriber()
    {
        _client = new MqttFactory().CreateMqttClient();

        _client.ApplicationMessageReceivedAsync += e =>
        {
            var json = e.ApplicationMessage
                .ConvertPayloadToString();

            var item = JsonSerializer.Deserialize<T>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (item != null)
                OnItemReceived?.Invoke(item);

            return Task.CompletedTask;
        };
    }

    public async Task ConnectAsync(string host, int port = 1883)
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(host, port)
            .WithClientId(Guid.NewGuid().ToString())
            .Build();

        await _client.ConnectAsync(options);
    }

    public async Task SubscribeAsync(string topic)
    {
        var options = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(topic)
            .Build();

        await _client.SubscribeAsync(options);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
