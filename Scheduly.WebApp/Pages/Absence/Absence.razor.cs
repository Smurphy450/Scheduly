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
        [Inject]
        private ISnackbar Snackbar { get; set; }
        [Inject]
        private AuthenticationStateProvider authStateProvider { get; set; }
        public int Index = -1; //default value cannot be 0 -> first selectedindex is 0.

        // TODO: Get absence type
        public double[] data = { 1, 2, 3, 4 };

        public string[] AbsenceArray = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var userId = 0; // temp
                string userName = string.Empty;

                var authState = await authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    userId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int a) ? a : 0;
                    userName = user.Identity.Name;

                    //TODO: Skal slettes herunder, bare for at vise hvordan det skal laves
                    if (userId == 0)
                    {
                        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                        Snackbar.Add("Failed to get user id from claims.", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                        Snackbar.Add($"Successfully retrieved user id from claims. UserID: {userId}, Username: {userName}", Severity.Success);
                    }
                }

                // TODO: Lav et api kald til at hente alle fraværestyper for en bruger, hente hvor mange gange de forkommer og indsæt det i data arrayet.
                // TODO: Den skal også kunne hente alt fravær for en bruger, tælle hvor mange gange de forskellige fraværestyper forkommer samt tælle op hvor mange timer fraværet er i alt.

                // TODO: Get amount of times the absence types have been used
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var getAllResponse = await httpClient.GetAsync($"https://localhost:7171/api/Absence/User/{userId}");
                        if (getAllResponse.IsSuccessStatusCode)
                        {
                            var content = await getAllResponse.Content.ReadAsStringAsync();

                            var absence = JsonConvert.DeserializeObject<List<Scheduly.WebApi.Models.Absence>>(content);
                            AbsenceArray = Array.ConvertAll(absence.ToArray(), x => x.ToString());

                            Console.WriteLine("Retrieved list of absence for user.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get all absence for user. Status: {getAllResponse.StatusCode}");
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
    }
}
