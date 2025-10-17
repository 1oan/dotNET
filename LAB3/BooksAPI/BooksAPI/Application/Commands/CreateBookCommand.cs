// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Commands/CreateBookCommand.cs
namespace BooksAPI.Application.Commands;

public record CreateBookCommand(string Title, string Author, int Year);

