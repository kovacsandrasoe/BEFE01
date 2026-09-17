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
            CreateMap<Book, BookViewDto>();
        }
    }
}
