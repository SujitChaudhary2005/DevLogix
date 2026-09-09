namespace DevLogix.Services;

using System.IO;

public static class ExecutionTimeLogger
{
    private static readonly object _lock = new object();

    public static void LogExecution(string actionDescription, long elapsedMs, DateTime timestamp)
    {
        // Assuming App_Data exists relative to the current working directory, 
        // usually better to inject IWebHostEnvironment, but this is a static demo.
        var logPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "execution-log.txt");

        try
        {
            lock (_lock)
            {
                // Ensure directory exists
                var dir = Path.GetDirectoryName(logPath);
                if (dir != null && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using var writer = new StreamWriter(logPath, append: true);
                writer.WriteLine($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] {actionDescription} took {elapsedMs}ms");
            }
        }
        catch
        {
            // Ignore logging exceptions to not crash the app
        }
    }
}
