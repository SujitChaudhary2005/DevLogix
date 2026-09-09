using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DevLogix.Middleware;

using DevLogix.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ITempDataDictionaryFactory tempDataDictionaryFactory)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            context.Response.Redirect("/Home/NotFound");
        }
        catch (ForbiddenException ex)
        {
            _logger.LogWarning(ex, "Forbidden access");
            context.Response.Redirect("/Account/AccessDenied");
        }
        catch (BusinessRuleException ex)
        {
            _logger.LogWarning(ex, "Business rule violation");
            var tempData = tempDataDictionaryFactory.GetTempData(context);
            tempData["ErrorMessage"] = ex.Message;
            
            string referer = context.Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                context.Response.Redirect(referer);
            }
            else
            {
                context.Response.Redirect("/");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            context.Response.Redirect("/Home/Error");
        }
    }
}
