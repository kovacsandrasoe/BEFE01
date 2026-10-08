using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using MyNamespace;

public class WebSocketClient<T>
{
    private readonly ClientWebSocket _socket = new();

    public event Action<T>? OnItemReceived;

    public async Task ConnectAsync(string url)
    {
        await _socket.ConnectAsync(
            new Uri(url),
            CancellationToken.None);
    }

    public async Task ListenAsync()
    {
        Console.WriteLine($"Socket state: {_socket.State}");

        var buffer = new byte[4096];

        while (_socket.State == WebSocketState.Open)
        {
            Console.WriteLine("Waiting for message...");
            using var stream = new MemoryStream();

            WebSocketReceiveResult result;

            do
            {
                result = await _socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _socket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closing",
                        CancellationToken.None);

                    return;
                }

                stream.Write(buffer, 0, result.Count);

            } while (!result.EndOfMessage);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var json = Encoding.UTF8.GetString(
                    stream.ToArray());

                var item = JsonSerializer.Deserialize<T>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (item != null)
                    OnItemReceived?.Invoke(item);
            }
        }
        Console.WriteLine($"Connection ended: {_socket.State}");
    }
}