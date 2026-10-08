using MyNamespace;
using System.Net.NetworkInformation;

namespace ClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client("https://localhost:7072", new HttpClient());

            var x = await client.BookAllAsync(0);



            using var mqttClient = new MqttSubscriber<BookShortViewDto>();

            mqttClient.OnItemReceived += book =>
            {
                Console.WriteLine($"New book: {book.Title}");
            };

            await mqttClient.ConnectAsync("localhost", 1883);

            await mqttClient.SubscribeAsync("books/created");

            Console.WriteLine("MQTT connected and subscribed!");
            Console.WriteLine("Press ENTER to exit...");

            Console.ReadLine();
        }
    }
}
