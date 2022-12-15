using System.Linq.Expressions;

namespace Shop.Api.Books.Tests.Helpers;

public class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{

    public AsyncEnumerable(IEnumerable<T> enumarable) : base(enumarable) { }
    public AsyncEnumerable(Expression expression) : base(expression) { }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }
      
    IQueryProvider IQueryable.Provider {
        get { return new AsyncQueryProvider<T>(this); }
    }

}