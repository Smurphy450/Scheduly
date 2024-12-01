using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Admin
{
    public class ApproveBookingBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

		protected List<ApproveBookingDTO> AllBookings { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetBookingsWithApprovalNeeded();
        }

        private async Task GetBookingsWithApprovalNeeded()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:7171/api/Bookings/PendingApproval");
                    if (response.IsSuccessStatusCode)
                    {
                        AllBookings = await response.Content.ReadFromJsonAsync<List<ApproveBookingDTO>>() ?? new List<ApproveBookingDTO>();
                    }
                    else
                    {
                        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                        Snackbar.Add("Failed to get pending approval bookings!", Severity.Error);

                        Console.WriteLine($"Failed to get pending approval bookings. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
            catch (Exception ex)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Error getting bookings!", Severity.Error);

                Console.WriteLine($"Error getting bookings: {ex.Message}");
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

