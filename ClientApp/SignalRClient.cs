using Microsoft.AspNetCore.SignalR.Client;

public class SignalRClient<T> : IAsyncDisposable
{
    private HubConnection? _connection;

    public event Action<T>? OnItemReceived;

    public async Task ConnectAsync(string url)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<T>("BookCreated", item =>
        {
            OnItemReceived?.Invoke(item);
        });

        await _connection.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
            await _connection.DisposeAsync();
    }
}