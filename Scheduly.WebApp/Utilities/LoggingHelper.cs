using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;

namespace Scheduly.WebApp.Utilities
{
    public static class LoggingHelper
    {
        public static async Task LogActionAsync(SchedulyContext context, LoggingDTO loggingDTO)
        {
            try
            {
                var logEntry = new SchedulyLogging
                {
                    UserId = loggingDTO.UserId,
                    Action = loggingDTO.Action,
                    AffectedData = loggingDTO.AffectedData
                };

                context.SchedulyLoggings.Add(logEntry);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging the action: {ex.Message}");
            }
        }
    }
}
