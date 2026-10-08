using MyNamespace;

namespace ClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client("https://localhost:7072", new HttpClient());

            var x = await client.BookAllAsync(0);
            

        }
    }
}
