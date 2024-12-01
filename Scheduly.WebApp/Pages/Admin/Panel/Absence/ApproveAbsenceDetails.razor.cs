using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models;
using Scheduly.WebApp.Pages.Booking;
using Scheduly.WebApp.Utilities;

namespace Scheduly.WebApp.Pages.Admin.Panel.Absence
{
	public class ApproveAbsenceDetailsBase : ComponentBase
	{
		[Parameter] public int AbsenceId { get; set; }

		[Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

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

        protected async Task ApproveAbsence(int id)
        {
            await UpdateAbsenceApproval(id, true);
        }

        protected async Task DisApproveAbsence(int id)
        {
            await UpdateAbsenceApproval(id, false);
        }

        private async Task UpdateAbsenceApproval(int id, bool isApproved)
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PutAsJsonAsync($"https://localhost:7171/api/Absence/Approve/{id}", isApproved);
                        if (response.IsSuccessStatusCode)
                        {
                            Snackbar.Add(isApproved ? "Absence approved successfully." : "Absence disapproved successfully.", Severity.Success);
                        }
                        else
                        {
                            Snackbar.Add("Failed to update Absence approval status.", Severity.Error);
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error when updating Absence approval status: {ex.Message}", Severity.Error);
                }
            }
        }
    }
}
