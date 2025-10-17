// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Handlers/DeleteBookHandler.cs
using BooksAPI.Application.Commands;
using BooksAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Application.Handlers;

public class DeleteBookHandler(BooksDbContext db)
{
    public async Task<bool> Handle(DeleteBookCommand cmd, CancellationToken ct = default)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == cmd.Id, ct);
        if (book is null) return false;
        db.Books.Remove(book);
        await db.SaveChangesAsync(ct);
        return true;
    }
}

