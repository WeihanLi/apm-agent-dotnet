using System;

namespace CMS.API.Campaign.Infrastructure.Logging
{
    public static class LoggingHelper
    {
        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Error(Exception ex, string methodName = null)
        {
            var name = !string.IsNullOrWhiteSpace(methodName) ? $"[{methodName}]" : string.Empty;
            Console.WriteLine($"{name} Exception Message: {ex.Message}, StackTrace: {ex.StackTrace}");
        }
    }
}
