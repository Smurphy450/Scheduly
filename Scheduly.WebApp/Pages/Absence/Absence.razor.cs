using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Absence
{
    public class AbsenceBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        protected List<AbsenceDTO> Absences { get; set; } = new List<AbsenceDTO>();
        protected DateTimeOffset startDate = DateTimeOffset.Now.AddMonths(-3);
        protected DateTimeOffset endDate = DateTimeOffset.Now.AddMonths(1);

        protected override async Task OnInitializedAsync()
        {
            await LoadAbsences();
        }

        protected async Task LoadAbsences()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                var query = new AbsenceQueryDTO
                {
                    UserId = userId,
                    StartDate = startDate,
                    EndDate = endDate
                };

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://localhost:7171/api/Absence/AbsenceDTOs", query);
                    if (response.IsSuccessStatusCode)
                    {
                        Absences = await response.Content.ReadFromJsonAsync<List<AbsenceDTO>>() ?? new List<AbsenceDTO>();
                    }
                    else
                    {
                        Console.WriteLine($"Failed to load absences. Status: {response.StatusCode}");
                    }
                }
            }
        }

        protected async Task DeleteAbsence(int absenceId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"https://localhost:7171/api/Absence/{absenceId}");
                if (response.IsSuccessStatusCode)
                {
                    Absences.RemoveAll(a => a.AbsenceId == absenceId);
                    StateHasChanged();
                }
                else
                {
                    Console.WriteLine($"Failed to delete absence. Status: {response.StatusCode}");
                }
            }
        }
    }
}
