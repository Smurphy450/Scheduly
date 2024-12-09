using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using System;
using System.Threading.Tasks;

namespace Scheduly.WebApi.Utilities
{
    public static class ErrorLoggingHelper
    {
        public static async Task LogErrorAsync(SchedulyContext context, int userId, string action, Exception ex)
        {
            var loggingDTO = new LoggingDTO
            {
                UserId = userId,
                Action = action,
                AffectedData = $"Error: {ex.Message}"
            };
            await LoggingHelper.LogActionAsync(context, loggingDTO);
        }
    }
}