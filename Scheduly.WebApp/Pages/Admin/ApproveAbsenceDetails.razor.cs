using Microsoft.AspNetCore.Components;

namespace Scheduly.WebApp.Pages.Admin
{
	public class ApproveAbsenceDetailsBase : ComponentBase
	{
		[Parameter] public int AbsenceId { get; set; }
		protected WebApi.Models.Absence model { get; set; } = new WebApi.Models.Absence();
	}
}
