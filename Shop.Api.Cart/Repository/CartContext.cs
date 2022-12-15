using Microsoft.EntityFrameworkCore;
using Shop.Api.Cart.Models;

namespace Shop.Api.Cart.Repository;

public class CartContext: DbContext
{
    public CartContext(DbContextOptions<CartContext> options) : base(options)
    {
    }

    public DbSet<CartSession> Session { get; set; }
    public DbSet<CartSessionDetail> SessionDetail { get; set; }
}