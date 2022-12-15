namespace Shop.Api.Cart.Application;

public class CartSessionDetailDto
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; }
    public string BookAuthor { get; set; }
    public DateTime PublishDate { get; set; }
}