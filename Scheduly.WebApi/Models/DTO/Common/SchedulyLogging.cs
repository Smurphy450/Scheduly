namespace Scheduly.WebApi.Models.DTO.Common
{
    public partial class SchedulyLogging
    {
        public int LogId { get; set; }

        public int UserId { get; set; }

        public string Action { get; set; } = null!;

        public string AffectedData { get; set; } = null!;

        public virtual Models.User User { get; set; } = null!;
    }
}