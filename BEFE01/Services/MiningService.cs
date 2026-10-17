using BEFE01.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace BEFE01.Services
{
    //Olyan szolgáltatás, amely egy megadott stringhez keres egy egész számot (nonce),
    //hogy a string és a szám összefűzéséből számított SHA-256 hash adott számú nullával kezdődjön.
    //Pl: Hello world és 6 bemenet esetén 40-50mp-es futásidő várható
    //a difficulty eggyel nagyobbra állítása kb. 16x-osára növeli a futásidőt
    public class MiningService
    {
        public MiningResult Mine(string text, int difficulty)
        {
            if (difficulty < 1 || difficulty > 8)
                throw new ArgumentOutOfRangeException(
                    nameof(difficulty));

            var prefix = new string('0', difficulty);
            var stopwatch = Stopwatch.StartNew();

            for (long nonce = 0; nonce < long.MaxValue; nonce++)
            {
                var input = Encoding.UTF8.GetBytes($"{text}{nonce}");
                var hash = SHA256.HashData(input);
                var hashString = Convert.ToHexString(hash);

                if (hashString.StartsWith(prefix))
                {
                    stopwatch.Stop();

                    return new MiningResult
                    {
                        Nonce = nonce,
                        Hash = hashString,
                        Iterations = nonce + 1,
                        ElapsedSeconds = stopwatch.Elapsed.TotalSeconds
                    };
                }
            }

            throw new InvalidOperationException("No result found.");
        }
    }
}
