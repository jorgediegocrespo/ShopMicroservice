using AutoMapper;
using Shop.Api.Authors.Models;

namespace Shop.Api.Authors.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Author, AuthorDto>();
    }
}