// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Handlers/GetBookByIdHandler.cs
using BooksAPI.Application.Queries;
using BooksAPI.Domain;
using BooksAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Application.Handlers;

public class GetBookByIdHandler(BooksDbContext db)
{
    public async Task<Book?> Handle(GetBookByIdQuery query, CancellationToken ct = default)
        => await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == query.Id, ct);
}

