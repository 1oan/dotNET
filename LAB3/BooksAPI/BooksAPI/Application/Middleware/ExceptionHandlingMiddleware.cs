// filepath: /home/ionut/RiderProjects/BooksAPI/BooksAPI/Application/Middleware/ExceptionHandlingMiddleware.cs
using System.Net;
using System.Text.Json;
using BooksAPI.Application.Exceptions;

namespace BooksAPI.Application.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            await WriteProblem(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteProblem(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblem(HttpContext ctx, HttpStatusCode status, string message)
    {
        ctx.Response.StatusCode = (int)status;
        ctx.Response.ContentType = "application/problem+json";
        var payload = new
        {
            type = "about:blank",
            title = Enum.GetName(typeof(HttpStatusCode), status),
            status = (int)status,
            detail = message,
            traceId = ctx.TraceIdentifier
        };
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}

