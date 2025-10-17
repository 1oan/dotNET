// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Commands/UpdateBookCommand.cs
namespace BooksAPI.Application.Commands;

public record UpdateBookCommand(int Id, string Title, string Author, int Year);

