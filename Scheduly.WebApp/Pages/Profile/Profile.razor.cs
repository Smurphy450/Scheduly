using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using Scheduly.WebApi.Models.DTO.User;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Text;
using Scheduly.WebApi.Models.DTO.AdminSettings;

namespace Scheduly.WebApp.Pages.Profile
{
    public class ProfileBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        protected UserProfileDTO model { get; set; } = new UserProfileDTO();
        protected List<AdminSettingDTO> AdminSettings { get; set; } = new List<AdminSettingDTO>();

        protected override async Task OnInitializedAsync()
        {
            await GetAdminSettings();
            await LoadUserProfile();
        }

        private async Task GetAdminSettings()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var getAllResponse = await httpClient.GetAsync("https://localhost:7171/api/AdminSettings");
                    if (getAllResponse.IsSuccessStatusCode)
                    {
                        var content = await getAllResponse.Content.ReadAsStringAsync();
                        AdminSettings = JsonConvert.DeserializeObject<List<AdminSettingDTO>>(content) ?? new List<AdminSettingDTO>();
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get admin settings. Status: {getAllResponse.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving admin settings: {ex.Message}");
            }
        }

        private async Task LoadUserProfile()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (userId != 0)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7171/api/Profiles/UserDto/{userId}");
                    if (response.IsSuccessStatusCode)
                    {
                        model = await response.Content.ReadFromJsonAsync<UserProfileDTO>() ?? new UserProfileDTO();
                    }
                    else
                    {
                        Console.WriteLine($"Failed to load user profile. Status: {response.StatusCode}");
                    }
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
                    var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync($"https://localhost:7171/api/Profiles/User?userId={userId}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Profile updated successfully.", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add("Failed to update profile.", Severity.Error);
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

        protected bool IsFieldEditable(int settingId)
        {
            var setting = AdminSettings.FirstOrDefault(s => s.SettingsId == settingId);
            return setting?.Enabled ?? false;
        }
    }
}
