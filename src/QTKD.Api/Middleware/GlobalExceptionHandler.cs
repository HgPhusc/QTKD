using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QTKD.Api.Exceptions;

namespace QTKD.Api.Middleware;

// Một nơi duy nhất chuyển exception thành phản hồi lỗi dạng ProblemDetails (RFC 7807).
// Phản hồi lỗi có thêm "rule" (mã quy tắc) và "errors" (danh sách điều kiện chưa thỏa) khi có.
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        ProblemDetails problem;

        if (exception is AppException app)
        {
            problem = new ProblemDetails
            {
                Status = app.StatusCode,
                Title = app switch
                {
                    NotFoundException => "Not Found",
                    ConflictException => "Conflict",
                    _ => "Business rule violated"
                },
                Detail = app.Message,
                Instance = context.Request.Path
            };
            if (app.Rule is not null) problem.Extensions["rule"] = app.Rule;
            if (app.Errors.Count > 0) problem.Extensions["errors"] = app.Errors;
        }
        else
        {
            logger.LogError(exception, "Unhandled exception");
            problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred.",
                Instance = context.Request.Path
            };
        }

        context.Response.StatusCode = problem.Status!.Value;
        await context.Response.WriteAsJsonAsync(problem, options: null,
            contentType: "application/problem+json", cancellationToken: ct);
        return true;
    }
}
