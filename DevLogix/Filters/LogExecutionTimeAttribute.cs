using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DevLogix.Filters;

public class LogExecutionTimeAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _actionDescription;
    public int ThresholdMs { get; set; } = 500;

    public LogExecutionTimeAttribute(string actionDescription)
    {
        _actionDescription = actionDescription;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LogExecutionTimeAttribute>>();
        var stopwatch = Stopwatch.StartNew();

        await next();

        stopwatch.Stop();
        if (stopwatch.ElapsedMilliseconds > ThresholdMs)
        {
            logger.LogWarning("Action {ActionDescription} took {ElapsedMilliseconds} ms, which exceeds the threshold of {ThresholdMs} ms.", 
                _actionDescription, stopwatch.ElapsedMilliseconds, ThresholdMs);
        }
    }
}
