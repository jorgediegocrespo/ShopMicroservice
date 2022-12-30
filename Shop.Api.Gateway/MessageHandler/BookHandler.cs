using System;
using System.Diagnostics;
using System.Text.Json;
using Shop.Api.Gateway.RemoteModels;
using Shop.Api.Gateway.RemoteServices;

namespace Shop.Api.Gateway.MessageHandler
{
	public class BookHandler : DelegatingHandler
	{
		private readonly ILogger<BookHandler> _logger;
        private readonly IAuthorService _authorService;

        public BookHandler(ILogger<BookHandler> logger, IAuthorService authorService)
        {
            _logger = logger;
            _authorService = authorService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var time = Stopwatch.StartNew();
            var responseBook = await base.SendAsync(request, cancellationToken);
            if (!responseBook.IsSuccessStatusCode)
            {
                time.Stop();
                _logger.LogInformation($"The process employed ${time.ElapsedMilliseconds} ms");
                return responseBook;

            }

            var content = await responseBook.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var book = JsonSerializer.Deserialize<RemoteBook>(content, options);
            var responseAuthor = await _authorService.GetAuthor(book!.AuthorGuid);
            if (responseAuthor.result)
            {
                book!.Author = responseAuthor.author;
                responseBook.Content = new StringContent(JsonSerializer.Serialize(book), System.Text.Encoding.UTF8, "application/json");
                time.Stop();
                _logger.LogInformation($"The process employed ${time.ElapsedMilliseconds} ms");
            }

            return responseBook;
        }
    }
}

