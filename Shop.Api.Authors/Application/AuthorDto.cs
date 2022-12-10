namespace Shop.Api.Authors.Application;

public class AuthorDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthday { get; set; }
    public string AuthorGuid { get; set; }
}