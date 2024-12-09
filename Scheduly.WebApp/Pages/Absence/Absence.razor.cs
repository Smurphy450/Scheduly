using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApi.Models.DTO.Absence;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Absence
{
    public class AbsenceBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        protected List<AbsenceDTO> Absences { get; set; } = new List<AbsenceDTO>(); //im getting a cannot convert from 'system.DateTimeOffset' to 'system.DateTime?'
        protected DateTime? startDate { get; set; } = DateTime.Now.AddMonths(-3);
        protected DateTime? endDate { get; set; } = DateTime.Now.AddMonths(1);

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
                    StartDate = (DateTimeOffset)startDate,
                    EndDate = (DateTimeOffset)endDate
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

        protected async Task UpdateAbsences()
        {
            await LoadAbsences();
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
