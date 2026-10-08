using BEFE01.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BEFE01.Services
{
    public class BookBackgroundService
    {
        public void Job(Guid authorId, BookDbContext ctx)
        {
            var author = ctx.People.First(t => t.Id == authorId);
            author.Name = author?.Name + "!";
            ctx.SaveChanges();
        }
    }
}
