using AutoMapper;
using Shop.Api.Books.Application;
using Shop.Api.Books.Models;

namespace Shop.Api.Books.Tests;

public class MappingTest : Profile
{
    public MappingTest() {
        CreateMap<Book, BookDto>();
    }

}