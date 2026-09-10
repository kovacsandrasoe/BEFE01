using BEFE01.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEFE01.Dtos
{
    public class BookViewDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Year { get; set; }

        public string AuthorName { get; set; } = string.Empty;
    }
}
