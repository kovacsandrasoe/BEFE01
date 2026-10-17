namespace BEFE01.Models
{
    public class MiningResult
    {
        public long Nonce { get; set; }
        public string Hash { get; set; } = "";
        public long Iterations { get; set; }
        public double ElapsedSeconds { get; set; }
    }
}
