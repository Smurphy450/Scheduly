using Microsoft.AspNetCore.Mvc;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.User;

namespace Scheduly.WebApi.Models.DTO.Common
{
    public class OverviewDataDTO
    {
        public bool DayStarted { get; set; }
        public double AverageWeeklyWorkTime { get; set; }
        public UserOverviewDTO UserOverview { get; set; }
    }
}

