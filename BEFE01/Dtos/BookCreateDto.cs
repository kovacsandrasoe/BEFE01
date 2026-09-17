namespace BEFE01.Dtos
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public Guid AuthorId { get; set; }
    }
}
