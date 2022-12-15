namespace Shop.Api.Cart.Application;

public class CartSessionDto
{
    public int CartSessionId { get; set; }
    public DateTime SessionCreationDate { get; set; }
    public List<CartSessionDetailDto> Details { get; set; }
}