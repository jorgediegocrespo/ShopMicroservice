using AutoMapper;
using Shop.Api.Books.Models;

namespace Shop.Api.Books.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>();
    }
}