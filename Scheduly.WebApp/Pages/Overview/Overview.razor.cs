using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Models.DTO;
using Scheduly.WebApp.Utilities;

namespace Scheduly.WebApp.Pages.Overview
{
    public class OverviewBase : ComponentBase
	{
		[Inject] private ISnackbar Snackbar { get; set; }
		[Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        protected bool DayStarted { get; set; } = false;
        protected double AverageWeeklyWorkTime { get; set; } = 0.0;
        protected List<OverviewPremisesDTO> AllPremises { get; set; } = new();
        protected List<OverviewResourcesDTO> AllResources { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await CheckDayStarted();
            await GetAverageWeeklyWorkTime();
            await GetUserOverview();
        }

        private async Task CheckDayStarted()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync($"https://localhost:7171/api/TimeRegistrations/Exists/{userId}");
                        if (response.IsSuccessStatusCode)
                        {
                            DayStarted = await response.Content.ReadFromJsonAsync<bool>();
                        }
                        else
                        {
                            Console.WriteLine($"Failed to check if day started. Status: {response.StatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"An error occurred while making the request: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking if day started: {ex.Message}");
                }
            }
        }

        private async Task GetAverageWeeklyWorkTime()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync($"https://localhost:7171/api/TimeRegistrations/AverageWeeklyWorkTime/{userId}");
                        if (response.IsSuccessStatusCode)
                        {
                            AverageWeeklyWorkTime = await response.Content.ReadFromJsonAsync<double>();
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get average weekly work time. Status: {response.StatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"An error occurred while making the request: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting average weekly work time: {ex.Message}");
                }
            }
        }

        private async Task GetUserOverview()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync($"https://localhost:7171/api/Bookings/GetUserOverview/{userId}");
                        if (response.IsSuccessStatusCode)
                        {
                            var userOverview = await response.Content.ReadFromJsonAsync<UserOverviewDTO>();
                            if (userOverview != null)
                            {
                                AllPremises = userOverview.OverviewPremises;
                                AllResources = userOverview.OverviewResources;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get user overview. Status: {response.StatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"An error occurred while making the request: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting user overview: {ex.Message}");
                }
            }
        }

        protected async Task StartDay()
        {
            await RegisterTime(true);
        }

        protected async Task EndDay()
        {
            await RegisterTime(false);
        }

        private async Task RegisterTime(bool isStart)
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                var timeRegistrationDto = new TimeRegistrationDTO
                {
                    UserId = userId,
                    Start = DateTimeOffset.Now,
                    End = isStart ? (DateTimeOffset?)null : DateTimeOffset.Now
                };

                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsJsonAsync("https://localhost:7171/api/TimeRegistrations/RegisterTime", timeRegistrationDto);
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Failed to register time. Status: {response.StatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"An error occurred while making the request: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error registering time: {ex.Message}");
                }
            }
            await CheckDayStarted();
		}

		protected async Task DeleteBooking(int bookingId)
		{
			try
			{
				using (var httpClient = new HttpClient())
				{
					var getAllResponse = await httpClient.DeleteAsync($"https://localhost:7171/api/Bookings/{bookingId}");
					if (getAllResponse.IsSuccessStatusCode)
					{
                        Snackbar.Add("Unbooked item.", Severity.Success);
						Console.WriteLine("Unbooked booking.");

						await GetUserOverview();
					}
					else
					{
						Console.WriteLine($"Failed to Delete booking. Status: {getAllResponse.StatusCode}");
					}
				}
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"An error occurred while making the request: {e.Message}");
			}
		}
	}
}
