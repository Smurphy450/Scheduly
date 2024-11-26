using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Routing.Constraints;
using MudBlazor;
using Newtonsoft.Json;
using Scheduly.WebApp.Authentication;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Absence
{
    public class AbsenceBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        public int Index = -1; //default value cannot be 0 -> first selectedindex is 0.

        // TODO: Get absence type
        public double[] data = { 1, 2, 3, 4 };

        public string[] AbsenceArray = [];

        protected override async Task OnInitializedAsync()
        {
            int userId = await GetUserInfo();
            if (userId > 0)
            {
                await GetAbsenceInfo(userId);
            }
        }

        private async Task GetAbsenceInfo(int userId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var getAllResponse = await httpClient.GetAsync($"https://localhost:7171/api/AbsenceTypes/UserAbsences/{userId}");
                        if (getAllResponse.IsSuccessStatusCode)
                        {
                            var content = await getAllResponse.Content.ReadAsStringAsync();
                            var absences = JsonConvert.DeserializeObject<List<WebApi.Models.UserAbsenceTypeDto>>(content);

                            if (absences.Count == 0)
                            {
                                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                                Snackbar.Add("No absences found for user.", Severity.Success);
                            }
                            else
                            {
                                // TODO: Du skal nok ikke bruge det arrray herunder, men "var absences" er en liste med de værdier som du vil bruge(UserAbsenceTypeDto), den består af; AbsenceTypeId, AbsenceTypeName, AbsenceCount, TotalMinutes

                                AbsenceArray = absences.Select(a => a.ToString()).ToArray();
                            }

                            Console.WriteLine("Retrieved list of absences for user.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get all absences for user. Status: {getAllResponse.StatusCode}");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"An error occurred while making the request: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving absence for user: {ex.Message}");
            }
        }

        private async Task<int> GetUserInfo()
        {
            try
            {
                var userId = 0;
                string userName = string.Empty;

                var authState = await authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    userId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int a) ? a : 0;
                    userName = user.Identity.Name;
                }

                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
                return 0;
            }
        }
    }
}
