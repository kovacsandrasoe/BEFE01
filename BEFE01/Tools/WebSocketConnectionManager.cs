using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

public class WebSocketConnectionManager
{
    private readonly ConcurrentDictionary<Guid, WebSocket>
        _clients = new();

    public Guid AddClient(WebSocket socket)
    {
        var id = Guid.NewGuid();
        _clients.TryAdd(id, socket);
        return id;
    }

    public void RemoveClient(Guid id)
    {
        _clients.TryRemove(id, out _);
    }

    public async Task BroadcastAsync<T>(T data)
    {
        var json = JsonSerializer.Serialize(data);
        var bytes = Encoding.UTF8.GetBytes(json);

        foreach (var client in _clients.Values)
        {
            if (client.State == WebSocketState.Open)
            {
                await client.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
        }
    }
}