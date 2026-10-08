using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

[ApiController]
[Route("[controller]")]
public class WebSocketController : ControllerBase
{
    private readonly WebSocketConnectionManager _manager;

    public WebSocketController(WebSocketConnectionManager manager)
    {
        _manager = manager;
    }

    [HttpGet("connect")]
    public async Task Connect()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            Response.StatusCode = 400;
            return;
        }

        using var socket =
            await HttpContext.WebSockets.AcceptWebSocketAsync();

        var id = _manager.AddClient(socket);
        var buffer = new byte[4096];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    HttpContext.RequestAborted);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closing",
                        CancellationToken.None);

                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // A kliens megszakította a kapcsolatot.
        }
        catch (WebSocketException)
        {
            // A kapcsolat váratlanul megszakadt.
        }
        finally
        {
            _manager.RemoveClient(id);
        }
    }
}