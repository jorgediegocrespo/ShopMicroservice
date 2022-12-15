using System.Text.Json;
using Shop.Api.Cart.RemoteModels;

namespace Shop.Api.Cart.RemoteServices;

public class BookService : IBookService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BookService> _logger;

    public BookService(IHttpClientFactory httpClientFactory, ILogger<BookService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool result, RemoteBook book, string errorMessage)> GetBook(Guid bookId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Books");
            var response = await client.GetAsync($"api/Book/{bookId}");
            if (!response.IsSuccessStatusCode) 
                return (false, null, response.ReasonPhrase);
        
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<RemoteBook>(content, options);
            return (true, result, null);
        }
        catch (Exception e)
        {
            _logger?.LogError(e.ToString());
            return (false, null, e.Message);
        }
    }
}