namespace Shop.Api.Authors.Models;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthday { get; set; }
    public ICollection<Degree> Degrees { get; set; }
    public string AuthorGuid { get; set; }
}