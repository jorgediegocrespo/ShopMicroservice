namespace Shop.Api.Authors.Models;

public class Degree
{
    public int DegreeId { get; set; }
    public string Name { get; set; }
    public string Center { get; set; }
    public DateTime FinishDate { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public string DegreeGuid { get; set; }
}