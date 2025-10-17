// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Common/PagedResult.cs
namespace BooksAPI.Application.Common;

public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => (int)System.Math.Ceiling((double)TotalCount / PageSize);
}

