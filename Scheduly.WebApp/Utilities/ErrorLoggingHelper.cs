using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;
using System;
using System.Threading.Tasks;

namespace Scheduly.WebApp.Utilities
{
    public static class ErrorLoggingHelper
    {
        public static async Task LogErrorAsync(SchedulyContext context, string action, Exception ex)
        {
            var loggingDTO = new LoggingDTO
            {
                UserId = 0,
                Action = action,
                AffectedData = $"Error: {ex.Message}"
            };
            await LoggingHelper.LogActionAsync(context, loggingDTO);
        }
    }
}
