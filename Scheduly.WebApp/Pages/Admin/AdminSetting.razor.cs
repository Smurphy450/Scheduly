using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;

namespace Scheduly.WebApp.Pages.Admin
{
    public class AdminSettingBase : ComponentBase
    {
        public List<AdminSettingDto> AdminSettingList { get; set; } = new List<AdminSettingDto>();

        protected override async Task OnInitializedAsync()
        {
            await GetAdminSettings();
        }
        protected void ToggleSetting(AdminSettingDto setting)
        {
            var existingSetting = AdminSettingList.FirstOrDefault(s => s.SettingsId == setting.SettingsId);
            if (existingSetting != null)
            {
                existingSetting.Enabled = !existingSetting.Enabled;
            }
        }

        private async Task GetAdminSettings()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var getAllResponse = await httpClient.GetAsync("https://localhost:7171/api/AdminSettings");
                        if (getAllResponse.IsSuccessStatusCode)
                        {
                            var content = await getAllResponse.Content.ReadAsStringAsync();
                            var adminSettings = JsonConvert.DeserializeObject<List<AdminSettingDto>>(content);
                            AdminSettingList = adminSettings ?? new List<AdminSettingDto>();

                            Console.WriteLine("Retrieved all admin settings.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get all admin settings. Status: {getAllResponse.StatusCode}");
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
                Console.WriteLine($"Error retrieving admin settings: {ex.Message}");
            }
        }
        protected async Task SubmitSettings()
        {
            await UpdateAdminSettings();
        }
        private async Task UpdateAdminSettings()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var putResponse = await httpClient.PutAsJsonAsync("https://localhost:7171/api/AdminSettings/UpdateList", AdminSettingList);
                        if (putResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Successfully updated admin settings.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to update admin settings. Status: {putResponse.StatusCode}");
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
                Console.WriteLine($"Error updating admin settings: {ex.Message}");
            }
        }
    }
}
