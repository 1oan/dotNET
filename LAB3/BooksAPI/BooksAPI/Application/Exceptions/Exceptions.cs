// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Exceptions/Exceptions.cs
namespace BooksAPI.Application.Exceptions;

public class BadRequestException(string message) : Exception(message);
public class NotFoundException(string message) : Exception(message);

