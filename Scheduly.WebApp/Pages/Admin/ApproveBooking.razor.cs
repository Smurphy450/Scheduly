using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Admin
{
    public class ApproveBookingBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }

        protected List<ApproveBookingDTO> AllBookings { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetBookingsWithApprovalNeeded();
        }

        private async Task GetBookingsWithApprovalNeeded()
        {
            try
            {
                var response = await HttpClient.GetAsync("api/Bookings/PendingApproval");
                if (response.IsSuccessStatusCode)
                {
                    AllBookings = await response.Content.ReadFromJsonAsync<List<ApproveBookingDTO>>() ?? new List<ApproveBookingDTO>();
                }
                else
                {
                    Console.WriteLine($"Failed to get pending approval bookings. Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting bookings: {ex.Message}");
            }
        }
    }
}

