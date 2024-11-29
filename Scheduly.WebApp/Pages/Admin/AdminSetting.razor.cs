﻿using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Admin
{
    public class AdminSettingBase : ComponentBase
    {
        public List<AdminSettingDTO> AdminSettingList { get; set; } = new List<AdminSettingDTO>();

        protected override async Task OnInitializedAsync()
        {
            await GetAdminSettings();
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
                        var adminSettings = JsonConvert.DeserializeObject<List<AdminSettingDTO>>(content);
                        AdminSettingList = adminSettings ?? new List<AdminSettingDTO>();

                        Console.WriteLine("Retrieved all admin settings.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get all admin settings. Status: {getAllResponse.StatusCode}");
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
            await GetAdminSettings(); // Update the front end after submitting settings
        }

        private async Task UpdateAdminSettings()
        {
            try
            {
                using (var httpClient = new HttpClient())
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating admin settings: {ex.Message}");
            }
        }

        protected void OnSwitchChanged(int settingsId, bool enabled)
        {
            //TODO: Fix så den opdaterer AdminSettingList

            Console.WriteLine($"Switch changed: SettingsId={settingsId}, Enabled={enabled}");
            var setting = AdminSettingList.FirstOrDefault(s => s.SettingsId == settingsId);
            if (setting != null)
            {
                setting.Enabled = enabled;
            }
        }
    }
}