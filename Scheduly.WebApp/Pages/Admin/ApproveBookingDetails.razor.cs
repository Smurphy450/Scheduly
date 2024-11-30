using Microsoft.AspNetCore.Components;

namespace Scheduly.WebApp.Pages.Admin
{
	public class ApproveBookingDetailsBase : ComponentBase
	{
		[Parameter] public int BookingId { get; set; }
	}
}
