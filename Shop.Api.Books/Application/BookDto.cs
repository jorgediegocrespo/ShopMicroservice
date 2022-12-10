namespace Shop.Api.Books.Application;

public class BookDto
{
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public Guid BookGuid { get; set; }
    public Guid AuthorGuid { get; set; }
}