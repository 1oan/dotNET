// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Queries/GetAllBooksQuery.cs
namespace BooksAPI.Application.Queries;

public record GetAllBooksQuery(
    string? Author = null,
    string? SortBy = null, // Allowed: "title", "year"
    bool Desc = false,
    int Page = 1,
    int PageSize = 10
);
