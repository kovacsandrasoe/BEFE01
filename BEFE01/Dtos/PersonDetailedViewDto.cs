namespace BEFE01.Dtos
{
    public class PersonDetailedViewDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public ICollection<BookShortViewDto> Books { get; set; } = new List<BookShortViewDto>();
    }
}
