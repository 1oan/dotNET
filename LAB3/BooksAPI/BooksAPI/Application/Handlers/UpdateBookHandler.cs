// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Handlers/UpdateBookHandler.cs
using BooksAPI.Application.Commands;
using BooksAPI.Application.Exceptions;
using BooksAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Application.Handlers;

public class UpdateBookHandler(BooksDbContext db)
{
    public async Task Handle(UpdateBookCommand cmd, CancellationToken ct = default)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == cmd.Id, ct);
        if (book is null)
            throw new NotFoundException($"Book with id {cmd.Id} not found.");

        book.Title = cmd.Title.Trim();
        book.Author = cmd.Author.Trim();
        book.Year = cmd.Year;

        await db.SaveChangesAsync(ct);
    }
}

