using Microsoft.AspNetCore.Components;
using Scheduly.WebApi.Models.DTO;
using System.Net.Http.Json;

namespace Scheduly.WebApp.Pages.Admin
{
    public class AdminOverviewBase : ComponentBase
    {
        public List<UserInfoDTO> UserList { get; set; } = new List<UserInfoDTO>();

        protected override async Task OnInitializedAsync()
        {
            await GetAllUsers();
        }

        private async Task GetAllUsers()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:7171/api/Users");
                    if (response.IsSuccessStatusCode)
                    {
                        var users = await response.Content.ReadFromJsonAsync<List<UserInfoDTO>>();
                        UserList = users ?? new List<UserInfoDTO>();

                        Console.WriteLine("Retrieved all users.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get all users. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }
    }
}
