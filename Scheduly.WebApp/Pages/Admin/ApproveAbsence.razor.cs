using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Admin
{
    public class ApproveAbsenceBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected List<ApproveAbsenceDTO> AllAbsence { get; set; } = new();

		protected override async Task OnInitializedAsync()
        {
            await GetAbsenceWithApprovalNeeded();
        }

        private async Task GetAbsenceWithApprovalNeeded()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:7171/api/Absence/PendingApproval");
                    if (response.IsSuccessStatusCode)
                    {
                        AllAbsence = await response.Content.ReadFromJsonAsync<List<ApproveAbsenceDTO>>() ?? new List<ApproveAbsenceDTO>();
                    }
                    else
                    {
                        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                        Snackbar.Add("Failed to get pending approval absences!", Severity.Error);

                        Console.WriteLine($"Failed to get pending approval absences. Status: {response.StatusCode}");
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
                Snackbar.Add("Error getting absences!", Severity.Error);

                Console.WriteLine($"Error getting absences: {ex.Message}");
            }
        }

        protected async Task ApproveAbsence(int id)
        {
			Snackbar.Add("It Works!", Severity.Success);
		}

		protected async Task DisApproveAbsence(int id)
		{
			Snackbar.Add("It Works!", Severity.Success);
		}
	}
}
