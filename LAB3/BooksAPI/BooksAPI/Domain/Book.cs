// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Domain/Book.cs
namespace BooksAPI.Domain;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
}

