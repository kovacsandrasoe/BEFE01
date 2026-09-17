using AutoMapper;
using BEFE01.Dtos;
using BEFE01.Models;

namespace BEFE01.Tools
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            //ide minden kell, amiről engedünk mappelést
            CreateMap<BookCreateDto, Book>();

            CreateMap<BookUpdateDto, Book>();

            CreateMap<Book, BookViewDto>()
                .AfterMap((entity, dto) =>
                {
                    //dto.AuthorName = entity.Author != null
                        //? entity.Author.Name : string.Empty;
                    dto.AuthorName = entity.Author?.Name ?? string.Empty;
                });
        }
    }
}
