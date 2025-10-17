// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Program.cs
using BooksAPI.Application.Commands;
using BooksAPI.Application.Handlers;
using BooksAPI.Application.Middleware;
using BooksAPI.Application.Queries;
using BooksAPI.Domain;
using BooksAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLite connection
var connString = builder.Configuration.GetConnectionString("Default")
                 ?? "Data Source=books.db";

builder.Services.AddDbContext<BooksDbContext>(options =>
    options.UseSqlite(connString));

// Register CQRS handlers
builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<GetBookByIdHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();
builder.Services.AddScoped<UpdateBookHandler>();

var app = builder.Build();

// Ensure DB exists
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BooksDbContext>();
    await db.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// global exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Minimal API endpoints (CQRS-based)

app.MapPost("/books", async (CreateBookRequest req, CreateBookHandler handler) =>
    {
        // Minimal validation as per assignment scope
        if (string.IsNullOrWhiteSpace(req.Title) || string.IsNullOrWhiteSpace(req.Author))
            return Results.BadRequest("Title and Author are required.");
        if (req.Year <= 0)
            return Results.BadRequest("Year must be a positive number.");

        var id = await handler.Handle(new CreateBookCommand(req.Title, req.Author, req.Year));
        return Results.Created($"/books/{id}", new { id });
    })
    .WithName("CreateBook")
    .WithOpenApi();

// Filtering, sorting (whitelisted), and pagination via query params
app.MapGet("/books", async (
        string? author,
        string? sortBy,
        bool? desc,
        int? page,
        int? pageSize,
        GetAllBooksHandler handler) =>
    {
        var result = await handler.Handle(new GetAllBooksQuery(
            Author: author,
            SortBy: sortBy,
            Desc: desc ?? false,
            Page: page ?? 1,
            PageSize: pageSize ?? 10));
        return Results.Ok(result);
    })
    .WithName("GetAllBooks")
    .WithOpenApi();

app.MapGet("/books/{id:int}", async (int id, GetBookByIdHandler handler) =>
    {
        var book = await handler.Handle(new GetBookByIdQuery(id));
        return book is not null ? Results.Ok(book) : Results.NotFound();
    })
    .WithName("GetBookById")
    .WithOpenApi();

// Update
app.MapPut("/books/{id:int}", async (int id, UpdateBookRequest req, UpdateBookHandler handler) =>
    {
        if (string.IsNullOrWhiteSpace(req.Title) || string.IsNullOrWhiteSpace(req.Author))
            return Results.BadRequest("Title and Author are required.");
        if (req.Year <= 0)
            return Results.BadRequest("Year must be a positive number.");

        await handler.Handle(new UpdateBookCommand(id, req.Title, req.Author, req.Year));
        return Results.NoContent();
    })
    .WithName("UpdateBook")
    .WithOpenApi();

app.MapDelete("/books/{id:int}", async (int id, DeleteBookHandler handler) =>
    {
        var deleted = await handler.Handle(new DeleteBookCommand(id));
        return deleted ? Results.NoContent() : Results.NotFound();
    })
    .WithName("DeleteBook")
    .WithOpenApi();

app.Run();

// Request DTOs kept near endpoints for simplicity
public record CreateBookRequest(string Title, string Author, int Year);
public record UpdateBookRequest(string Title, string Author, int Year);
