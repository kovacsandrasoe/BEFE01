using MyNamespace;

namespace ClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client("https://localhost:7072", new HttpClient());

            var x = await client.BookAllAsync(0);

            await using var signalRClient = new SignalRClient<BookShortViewDto>();

            signalRClient.OnItemReceived += book =>
            {
                Console.WriteLine($"New book: {book.Title}");
            };

            await signalRClient.ConnectAsync(
                "https://localhost:7072/bookhub");

            Console.WriteLine("Connected!");
            Console.WriteLine("Press ENTER to exit...");

            Console.ReadLine();
        }
    }
}
