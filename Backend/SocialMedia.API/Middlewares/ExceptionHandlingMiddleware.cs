using SocialMedia.Application.Helpers;

namespace SocialMedia.API.Middlewares;
public class ExceptionHandlingMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteResponse(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (BadRequestException ex)
        {
            await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            await WriteResponse(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (AppException ex)
        {
            await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            await WriteResponse(context, StatusCodes.Status500InternalServerError, "Something went wrong");
        }
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = message });
    }
}
