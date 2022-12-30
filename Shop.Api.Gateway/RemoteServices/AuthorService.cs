using System;
using System.Text.Json;
using Shop.Api.Gateway.RemoteModels;

namespace Shop.Api.Gateway.RemoteServices;

public class AuthorService : IAuthorService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IHttpClientFactory httpClientFactory, ILogger<AuthorService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool result, RemoteAuthor author, string errorMessage)> GetAuthor(Guid authorId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Authors");
            var response = await client.GetAsync($"Author/{authorId}");
            if (!response.IsSuccessStatusCode)
                return (false, null, response.ReasonPhrase);

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<RemoteAuthor>(content, options);
            return (true, result, null);
        }
        catch (Exception e)
        {
            _logger?.LogError(e.ToString());
            return (false, null, e.Message);
        }
    }
}

