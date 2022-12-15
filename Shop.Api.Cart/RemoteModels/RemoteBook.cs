namespace Shop.Api.Cart.RemoteModels;

public class RemoteBook
{
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public Guid BookGuid { get; set; }
    public Guid AuthorGuid { get; set; }
}