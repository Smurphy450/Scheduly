using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;

namespace Scheduly.WebApp.Pages.Admin
{
    public class ApproveAbsenceBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        protected List<ApproveAbsenceDTO> AllAbsence { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetAbsenceWithApprovalNeeded();
        }

        private async Task GetAbsenceWithApprovalNeeded()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                try
                {
                    //using (var httpClient = new HttpClient())
                    //{
                    //	var response = await httpClient.GetAsync($"https://localhost:7171/api/Bookings/GetUserOverview/{userId}");
                    //	if (response.IsSuccessStatusCode)
                    //	{
                    //		var userOverview = await response.Content.ReadFromJsonAsync<UserOverviewDTO>();
                    //		if (userOverview != null)
                    //		{
                    //			AllPremises = userOverview.OverviewPremises;
                    //			AllResources = userOverview.OverviewResources;
                    //		}
                    //	}
                    //	else
                    //	{
                    //		Console.WriteLine($"Failed to get user overview. Status: {response.StatusCode}");
                    //	}
                    //}
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"An error occurred while making the request: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting absence: {ex.Message}");
                }
            }
        }
    }
}
