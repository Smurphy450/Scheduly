using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using System.Threading.Tasks;

namespace Scheduly.WebApi.Utilities
{
    public static class LoggingHelper
    {
        public static async Task LogActionAsync(SchedulyContext context, LoggingDTO loggingDTO)
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
    }
}