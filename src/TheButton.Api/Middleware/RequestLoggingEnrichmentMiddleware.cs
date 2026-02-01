using System.Diagnostics;
using Microsoft.Extensions.Primitives;

namespace TheButton.Api.Middleware;

/// <summary>
/// Enriches request logs with correlation fields and a completion log entry.
/// </summary>
/// <param name="_next">The next middleware in the pipeline.</param>
/// <param name="_logger">The logger for request completion entries.</param>
internal sealed class RequestLoggingEnrichmentMiddleware(
    RequestDelegate _next,
    ILogger<RequestLoggingEnrichmentMiddleware> _logger)
{
    private const string _idempotencyKeyHeader = "Idempotency-Key";

    private static readonly Action<ILogger, int, double, Exception?> _requestCompletedLog =
        LoggerMessage.Define<int, double>(
            LogLevel.Information,
            new EventId(1001, "RequestCompleted"),
            "Request completed with status {StatusCode} in {ElapsedMs}ms");

    /// <summary>
    /// Enriches the request log scope and logs completion with elapsed time.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        string traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
        string operation = GetOperation(context);
        string? userId = GetUserId(context);
        string? idempotencyKey = GetIdempotencyKey(context);

        using (_logger.BeginScope(new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["traceId"] = traceId,
            ["operation"] = operation,
            ["userId"] = userId,
            ["idempotencyKey"] = idempotencyKey,
        }))
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            finally
            {
                stopwatch.Stop();
                _requestCompletedLog(
                    _logger,
                    context.Response.StatusCode,
                    Math.Round(stopwatch.Elapsed.TotalMilliseconds, 2),
                    null);
            }
        }
    }

    private static string GetOperation(HttpContext context)
    {
        Endpoint? endpoint = context.GetEndpoint();
        if (!string.IsNullOrWhiteSpace(endpoint?.DisplayName))
        {
            return endpoint.DisplayName!;
        }

        return $"{context.Request.Method} {context.Request.Path}";
    }

    private static string? GetUserId(HttpContext context)
    {
        if (context.Request.RouteValues.TryGetValue("userId", out object? routeValue) && routeValue is not null)
        {
            return routeValue.ToString();
        }

        if (context.Request.Query.TryGetValue("userId", out StringValues queryValue)
            && !StringValues.IsNullOrEmpty(queryValue))
        {
            return queryValue.ToString();
        }

        PathString path = context.Request.Path;
        if (path.HasValue)
        {
            string[] segments = path.Value!.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string lastSegment = segments.Length > 0 ? segments[^1] : string.Empty;
            if (Guid.TryParse(lastSegment, out Guid parsedUserId))
            {
                return parsedUserId.ToString();
            }
        }

        return null;
    }

    private static string? GetIdempotencyKey(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_idempotencyKeyHeader, out StringValues headerValue)
            && !StringValues.IsNullOrEmpty(headerValue))
        {
            return headerValue.ToString();
        }

        return null;
    }
}
