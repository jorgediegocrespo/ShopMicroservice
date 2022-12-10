using Microsoft.EntityFrameworkCore;
using Shop.Api.Books.Models;

namespace Shop.Api.Books.Repository;

public class BookContext: DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
}