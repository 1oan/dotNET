// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Infrastructure/BooksDbContext.cs
using BooksAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Infrastructure;

public class BooksDbContext(DbContextOptions<BooksDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
}

