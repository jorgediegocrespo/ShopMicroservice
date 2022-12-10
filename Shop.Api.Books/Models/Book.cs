namespace Shop.Api.Books.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public Guid BookGuid { get; set; }
    public Guid AuthorGuid { get; set; }
}