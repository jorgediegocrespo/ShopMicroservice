using System;
namespace Shop.Api.Gateway.RemoteModels;

public class RemoteBook
{
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public Guid BookGuid { get; set; }
    public Guid AuthorGuid { get; set; }
    public RemoteAuthor Author { get; set; }
}

