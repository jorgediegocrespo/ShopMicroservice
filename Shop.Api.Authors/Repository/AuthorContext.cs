using Microsoft.EntityFrameworkCore;
using Shop.Api.Authors.Models;

namespace Shop.Api.Authors.Repository;

public class AuthorContext : DbContext
{
    public AuthorContext(DbContextOptions<AuthorContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Degree> Degrees { get; set; }
}