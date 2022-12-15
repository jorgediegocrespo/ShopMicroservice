using Shop.Api.Cart.RemoteModels;

namespace Shop.Api.Cart.RemoteServices;

public interface IBookService
{
    Task<(bool result, RemoteBook book, string errorMessage)> GetBook(Guid bookId);
}