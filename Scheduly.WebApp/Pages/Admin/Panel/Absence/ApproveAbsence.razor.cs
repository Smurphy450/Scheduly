using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO.Absence;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Admin.Panel.Absence
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
