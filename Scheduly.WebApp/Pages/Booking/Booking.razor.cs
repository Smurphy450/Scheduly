using Microsoft.AspNetCore.Components;
using Scheduly.WebApi.Models;

namespace Scheduly.WebApp.Pages.Booking
{
	public class BookingBase : ComponentBase
	{
		public WebApi.Models.Booking bookingModel = new WebApi.Models.Booking();

		GetBookings();
	}
}
