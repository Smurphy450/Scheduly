using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Scheduly.WebApi.Models;
using Scheduly.WebApp.Pages.Booking;
using Scheduly.WebApp.Utilities;

namespace Scheduly.WebApp.Pages.Admin
{
	public class ApproveAbsenceDetailsBase : ComponentBase
	{
		[Parameter] public int AbsenceId { get; set; }

		[Inject] private AuthenticationStateProvider authStateProvider { get; set; }

		protected WebApi.Models.DTO.ApproveAbsenceDTO model { get; set; } = new WebApi.Models.DTO.ApproveAbsenceDTO();
		
		protected override async Task OnInitializedAsync()
		{
			await GetAbsenceDetails();
		}

		private async Task GetAbsenceDetails()
		{
			var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
			if (userId != 0)
			{
				try
				{
					using (var httpClient = new HttpClient())
					{
						var response = await httpClient.GetAsync($"https://localhost:7171/api/Absence/ApproveAbsence/{AbsenceId}");
						if (response.IsSuccessStatusCode)
						{
							var absence = await response.Content.ReadFromJsonAsync<WebApi.Models.DTO.ApproveAbsenceDTO>();
							model = absence ?? new WebApi.Models.DTO.ApproveAbsenceDTO();
						}
						else
						{
							Console.WriteLine($"Failed to get absence info. Status: {response.StatusCode}");
						}
					}
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine($"An error occurred while making the request: {e.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error when retrieving absence info: {ex.Message}");
				}
			}
        }
	}
}
