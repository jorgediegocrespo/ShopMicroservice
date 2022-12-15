using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shop.Api.Books.Application;
using Shop.Api.Books.Models;
using Shop.Api.Books.Repository;
using Shop.Api.Books.Tests.Helpers;

namespace Shop.Api.Books.Tests;

public class BookServiceTest
{
    // private IEnumerable<Book> GetTestData()
    // {
    //
    //     A.Configure<Book>()
    //         .Fill(x => x.BookId).
    //         .Fill(x => x.Title).AsArticleTitle()
    //         .Fill(x => x.BookGuid, () => { return Guid.NewGuid(); })
    //         .Fill();
    //
    //
    //     var data = A.ListOf<Book>(30);
    //     data[0].BookGuid = Guid.Empty;
    //
    //     return data;   
    // }

    private async Task<BookContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<BookContext>()
            .UseInMemoryDatabase(databaseName: "BooksDatabase")
            .Options;

        var context = new BookContext(options);
        var books = new Book[]
        {
            new Book() { BookId = 1, Title = "Cien años de soledad", PublishDate = new DateTime(1954, 10, 23), BookGuid = Guid.NewGuid(), AuthorGuid = new Guid() },
            new Book() { BookId = 2, Title = "El código Da Vinci", PublishDate = new DateTime(1998, 8, 2), BookGuid = Guid.Empty, AuthorGuid = new Guid() },
            new Book() { BookId = 3, Title = "El nombre de la rosa", PublishDate = new DateTime(1942, 4, 13), BookGuid = new Guid(), AuthorGuid = new Guid() },
            new Book() { BookId = 4, Title = "El guardián entre el centeno", PublishDate = new DateTime(1976, 1, 29), BookGuid = new Guid(), AuthorGuid = new Guid() },
            new Book() { BookId = 5, Title = "El alquimista", PublishDate = new DateTime(1890, 7, 18), BookGuid = new Guid(), AuthorGuid = new Guid() },
        };
        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();

        return context;
    }

    // private Mock<BookContext> CreateMockedContext() 
    // {
    //     var testData = GetTestData().AsQueryable();
    //     
    //     var dbSet = new Mock<DbSet<Book>>();
    //     dbSet.As<IQueryable<Book>>().Setup(x => x.Provider).Returns(testData.Provider);
    //     dbSet.As<IQueryable<Book>>().Setup(x => x.Expression).Returns(testData.Expression);
    //     dbSet.As<IQueryable<Book>>().Setup(x => x.ElementType).Returns(testData.ElementType);
    //     dbSet.As<IQueryable<Book>>().Setup(x => x.GetEnumerator()).Returns(testData.GetEnumerator());
    //     
    //     dbSet.As<IAsyncEnumerable<Book>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
    //     .Returns(new AsyncEnumerator<Book>(testData.GetEnumerator()));
    //     
    //     dbSet.As<IQueryable<Book>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<Book>(testData.Provider));
    //     
    //     var context = new Mock<BookContext>();
    //     context.Setup(x => x.Books).Returns(dbSet.Object);
    //     return context;
    // }
    
    [Fact]
    public async void GetBookByGuid() 
    {
        await using var context = await CreateContextAsync();
        var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingTest()));
        var mapper = mapConfig.CreateMapper();

        var request = new QueryBookByGuid.BookByGuid();
        request.BookGuid = Guid.Empty.ToString();

        var handler = new QueryBookByGuid.Handler(context, mapper);
        var book = await  handler.Handle(request, new System.Threading.CancellationToken());

        Assert.NotNull(book);
        Assert.True(book.BookGuid == Guid.Empty);
        Assert.True(book.Title == "El código Da Vinci");

        await context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async void GetBooks() 
    {
        await using var context = await CreateContextAsync();
        var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingTest()));
        var mapper = mapConfig.CreateMapper();

        var request = new QueryBook.BookList();
        
        var handler = new QueryBook.Handler(context, mapper);
        var bookList = await handler.Handle(request, new System.Threading.CancellationToken());

        Assert.True(bookList.Any());
        
        await context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async void NewBook() 
    {
        await using var context = await CreateContextAsync();

        var request = new NewBook.Execute();
        request.Title = "Test title";
        request.AuthorGuid = Guid.Empty;
        request.PublishDate = DateTime.Now;

        var handler = new NewBook.Handler(context);

        var book = await handler.Handle(request, new System.Threading.CancellationToken());

        Assert.True(book != null);
        
        await context.Database.EnsureDeletedAsync();
    }
}