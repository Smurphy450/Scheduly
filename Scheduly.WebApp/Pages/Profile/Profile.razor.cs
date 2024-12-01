using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;
using Scheduly.WebApp.Utilities;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Scheduly.WebApp.Pages.Profile
{
    public class ProfileBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        protected ProfileDTO model { get; set; } = new ProfileDTO();

        protected override async Task OnInitializedAsync()
        {
            //TODO: hent admin setting, og brug det til at bestemme hvilke ting de må rette ved.
            await GetAllProfileInfo();
        }

        private async Task GetAllProfileInfo()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync($"https://localhost:7171/api/Profiles/UserDto/{userId}");
                        if (response.IsSuccessStatusCode)
                        {
                            var profile = await response.Content.ReadFromJsonAsync<ProfileDTO>();
                            model = profile ?? new ProfileDTO();
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

        protected async Task SaveChanges()
        {
            //TODO: skal være validering på, vi skal være sikker på at dataen er i orden, f.eks. at man ikke bare har tømt alle feltre.
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync($"https://localhost:7171/api/Profiles/User/{model.UserId}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Profile updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to update profile. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when updating profile: {ex.Message}");
            }
        }
    }
}

