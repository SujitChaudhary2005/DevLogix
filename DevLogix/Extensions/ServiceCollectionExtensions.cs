namespace DevLogix.Extensions;

using DevLogix.Authorization;
using DevLogix.Services;
using DevLogix.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ITimeLogService, TimeLogService>();

        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IExportService, ExportService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
        services.AddScoped<IAuthorizationHandler, CanManageProjectHandler>();

        return services;
    }
}
