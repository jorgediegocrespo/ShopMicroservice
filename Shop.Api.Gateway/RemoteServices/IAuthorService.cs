using Shop.Api.Gateway.RemoteModels;

namespace Shop.Api.Gateway.RemoteServices;

public interface IAuthorService
{
    Task<(bool result, RemoteAuthor author, string errorMessage)> GetAuthor(Guid authorId);
}

