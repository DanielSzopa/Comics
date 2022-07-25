using Comics.ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Comics.ApplicationCore.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException ex)
        {
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Bad request");
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
