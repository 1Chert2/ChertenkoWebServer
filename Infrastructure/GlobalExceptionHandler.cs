using System.Diagnostics;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Text.Json;
using ChertenkoWebServer.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace ChertenkoWebServer.Infrastructure;

public static class GlobalExceptionHandler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// Глобальный обработчик исключений: для /api/* возвращает JSON { error: { message, code } },
    /// для остальных запросов перенаправляет на страницу /Error.
    /// </summary>
    public static void Configure(IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            // Логируем исключение
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("GlobalExceptionHandler");
            logger.LogError(exception, "Необработанное исключение: {Path}", context.Request.Path);

            context.Response.Clear();

            var isApiRequest = context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);

            if (isApiRequest)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = $"{MediaTypeNames.Application.Json}; charset=utf-8";

                var apiError = new ApiError(
                    ApiErrorMessages.InternalServerError,
                    ApiErrorCodes.InternalServerError);

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new { error = apiError }, JsonOptions));
            }
            else
            {
                context.Response.Redirect("/Error");
            }
        });
    }
}