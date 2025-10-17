// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Handlers/GetAllBooksHandler.cs
using BooksAPI.Application.Common;
using BooksAPI.Application.Exceptions;
using BooksAPI.Application.Queries;
using BooksAPI.Domain;
using BooksAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Application.Handlers;

public class GetAllBooksHandler(BooksDbContext db)
{
    public async Task<PagedResult<Book>> Handle(GetAllBooksQuery q, CancellationToken ct = default)
    {
        // base query
        IQueryable<Book> query = db.Books.AsNoTracking();

        // filtering
        if (!string.IsNullOrWhiteSpace(q.Author))
        {
            var author = q.Author.Trim();
            query = query.Where(b => EF.Functions.Like(b.Author, $"%{author}%"));
        }

        // total count before pagination
        var total = await query.CountAsync(ct);

        // sorting with whitelist to prevent arbitrary property injection
        var sortKey = (q.SortBy ?? "id").ToLowerInvariant();
        var desc = q.Desc;
        query = sortKey switch
        {
            "title" => desc ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
            "year" => desc ? query.OrderByDescending(b => b.Year) : query.OrderBy(b => b.Year),
            "id" or "" => desc ? query.OrderByDescending(b => b.Id) : query.OrderBy(b => b.Id),
            _ => throw new BadRequestException("Invalid sort field. Allowed: id, title, year.")
        };

        // validate pagination
        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = q.PageSize is < 1 or > 100 ? 10 : q.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Book>(items, total, page, pageSize);
    }
}
