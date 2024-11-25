using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Absence
{
    public class ReportAbsenceBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        public int AbsenceTypeId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public string Description { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserId();
                await SaveAbsenceToDb();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error retrieving absence for user: {ex.Message}");
            }
        }

        private static async Task SaveAbsenceToDb()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    //var getAllResponse = await httpClient.GetAsync($"https://localhost:7171/api/Absence/User/{UserId}");
                    //if (getAllResponse.IsSuccessStatusCode)
                    //{
                    //    var content = await getAllResponse.Content.ReadAsStringAsync();

                    //    var absence = JsonConvert.DeserializeObject<List<WebApi.Models.Absence>>(content);
                    //    AbsenceArray = Array.ConvertAll(absence.ToArray(), x => x.ToString());

                    //    Console.WriteLine("Retrieved list of absence for user.");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Failed to get all absence for user. Status: {getAllResponse.StatusCode}");
                    //}
                }
                catch (HttpRequestException e)
                {
                    //Console.WriteLine($"An error occurred while making the request: {e.Message}");
                }
            }
        }

        private async Task GetUserId()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                UserId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int a) ? a : 0;
            }
        }
    }
}
