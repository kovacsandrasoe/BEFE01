using System.ComponentModel.DataAnnotations;

namespace BEFE01.Dtos
{
    public class BookCreateDto
    {
        [MinLength(3)]
        public string Title { get; set; } = string.Empty;

        [Range(1500,2500)]
        public int Year { get; set; }

        public Guid AuthorId { get; set; }
    }
}
