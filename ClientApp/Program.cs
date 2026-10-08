using MyNamespace;

namespace ClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client("https://localhost:7072", new HttpClient());

            var x = await client.BookAllAsync(0);

            var wsClient = new WebSocketClient<BookShortViewDto>();
            wsClient.OnItemReceived += book =>
            {
                Console.WriteLine($"New book: {book.Title}");
            };

            await wsClient.ConnectAsync(
                "wss://localhost:7072/websocket/connect");

            Console.WriteLine("Connected!");

            await wsClient.ListenAsync();

            Console.ReadLine();
        }
    }
}
