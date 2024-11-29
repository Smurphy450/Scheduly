using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Scheduly.WebApp.Pages.Profile
{
    public class ProfileBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        protected ProfileDTO model { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAllProfileInfo();
        }

        private async Task GetAllProfileInfo()
        {
            var userId = await GetUserInfo();
            if (userId != 0)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        // TODO: Get data from the ZipCode & Users tables
                        var response = await httpClient.GetAsync($"https://localhost:7171/api/Profiles/User/{userId}");
                        if (response.IsSuccessStatusCode)
                        {
                            var profile = await response.Content.ReadFromJsonAsync<ProfileDTO>();
                            // TODO: save profile data to the model
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get profile info. Status: {response.StatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"An error occurred while making the request: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when retrieving profile info: {ex.Message}");
                }
            }
        }

        // TODO: Create logic for saving changes
        protected async Task SaveChanges()
        {
            //await Http.PostAsJsonAsync("api/profile", model);
            // Handle success or failure
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
