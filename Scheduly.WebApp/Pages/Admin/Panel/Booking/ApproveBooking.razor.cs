using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO.Booking;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Admin.Panel.Booking
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

        protected async Task ApproveBooking(int id)
        {
            await UpdateBookingApproval(id, true);
        }

        protected async Task DisApproveBooking(int id)
        {
            await UpdateBookingApproval(id, false);
        }

        private async Task UpdateBookingApproval(int id, bool isApproved)
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PutAsJsonAsync($"https://localhost:7171/api/Bookings/Booking/{id}", isApproved);
                        if (response.IsSuccessStatusCode)
                        {
                            Snackbar.Add(isApproved ? "Booking approved successfully." : "Booking disapproved successfully.", Severity.Success);
                        }
                        else
                        {
                            Snackbar.Add("Failed to update booking approval status.", Severity.Error);
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error when updating booking approval status: {ex.Message}", Severity.Error);
                }
            }
        }
    }
}

