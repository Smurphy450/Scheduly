using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApp.Models.DTO;
using Scheduly.WebApp.Utilities;

namespace Scheduly.WebApp.Pages.TimeRegistration
{
	public class TimeRegistrationBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        protected List<TimeRegistrationDTO> TimeRegistration { get; set; } = new List<TimeRegistrationDTO>();
        protected DateTimeOffset startDate = DateTimeOffset.Now.AddMonths(-3);
        protected DateTimeOffset endDate = DateTimeOffset.Now.AddMonths(1);

        protected override async Task OnInitializedAsync()
        {
            await LoadTimeRegistrations();
        }

        protected async Task LoadTimeRegistrations()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                var query = new TimeRegistrationDTO
                {
                    UserId = userId,
                    Start = startDate,
                    End = endDate
                };

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://localhost:7171/api/TimeRegistrations/TimeRegistrationDTOs", query);
                    if (response.IsSuccessStatusCode)
                    {
                        TimeRegistration = await response.Content.ReadFromJsonAsync<List<TimeRegistrationDTO>>() ?? new List<TimeRegistrationDTO>();
                    }
                    else
                    {
                        Console.WriteLine($"Failed to load absences. Status: {response.StatusCode}");
                    }
                }
            }
        }

        protected async Task DeleteTime(int timeId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"https://localhost:7171/api/TimeRegistrations/{timeId}");
                if (response.IsSuccessStatusCode)
                {
                    TimeRegistration.RemoveAll(a => a.TimeId == timeId);
                    StateHasChanged();
                }
                else
                {
                    Console.WriteLine($"Failed to delete time registration. Status: {response.StatusCode}");
                }
            }
        }
    }
}
