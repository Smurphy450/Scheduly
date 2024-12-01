using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApp.Utilities;

namespace Scheduly.WebApp.Pages.Admin
{
	public class ApproveBookingDetailsBase : ComponentBase
	{
		[Parameter] public int BookingId { get; set; }

		[Inject] private AuthenticationStateProvider authStateProvider { get; set; }
		[Inject] private ISnackbar Snackbar { get; set; }

		protected WebApi.Models.DTO.ApproveBookingDTO model { get; set; } = new WebApi.Models.DTO.ApproveBookingDTO();

		protected override async Task OnInitializedAsync()
		{
			await GetBookingDetails();
		}

		private async Task GetBookingDetails()
		{
			var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
			if (userId != 0)
			{
				try
				{
					using (var httpClient = new HttpClient())
					{
						var response = await httpClient.GetAsync($"https://localhost:7171/api/Bookings/ApproveBooking/{BookingId}");
						if (response.IsSuccessStatusCode)
						{
							var absence = await response.Content.ReadFromJsonAsync<WebApi.Models.DTO.ApproveBookingDTO>();
							model = absence ?? new WebApi.Models.DTO.ApproveBookingDTO();
						}
						else
						{
							Console.WriteLine($"Failed to get booking info. Status: {response.StatusCode}");
						}
					}
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine($"An error occurred while making the request: {e.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error when retrieving booking info: {ex.Message}");
				}
			}
		}

		protected async Task ApproveBooking()
		{
			Snackbar.Add("It Works!", Severity.Success);
		}

		protected async Task DisApproveBooking()
		{
			Snackbar.Add("It Works!", Severity.Success);
		}
	}
}
