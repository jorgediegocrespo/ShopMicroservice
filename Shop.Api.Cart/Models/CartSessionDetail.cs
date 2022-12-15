namespace Shop.Api.Cart.Models;

public class CartSessionDetail
{
    public int CartSessionDetailId { get; set; }
    public DateTime CreationDate { get; set; }
    public string SelectedProduct { get; set; }
    public int CartSessionId { get; set; }
    public CartSession CartSession { get; set; }
}