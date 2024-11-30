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
        protected async Task DeleteUser(int userId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync($"https://localhost:7171/api/Users/{userId}");
                    if (response.IsSuccessStatusCode)
                    {
                        UserList = UserList.Where(u => u.UserId != userId).ToList();
                        Console.WriteLine("User deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete user. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        protected void EditUser(int userId)
        {
            // Navigate to the edit user page or open a dialog for editing
            // For example:
            // NavigationManager.NavigateTo($"/EditUser/{userId}");
        }
    }
}
