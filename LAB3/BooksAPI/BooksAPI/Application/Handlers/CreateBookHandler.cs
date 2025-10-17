// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Handlers/CreateBookHandler.cs
using BooksAPI.Application.Commands;
using BooksAPI.Domain;
using BooksAPI.Infrastructure;

namespace BooksAPI.Application.Handlers;

public class CreateBookHandler(BooksDbContext db)
{
    public async Task<int> Handle(CreateBookCommand cmd, CancellationToken ct = default)
    {
        var book = new Book
        {
            Title = cmd.Title.Trim(),
            Author = cmd.Author.Trim(),
            Year = cmd.Year
        };
        db.Books.Add(book);
        await db.SaveChangesAsync(ct);
        return book.Id;
    }
}

