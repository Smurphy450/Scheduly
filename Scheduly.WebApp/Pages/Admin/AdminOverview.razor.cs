using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Scheduly.WebApi.Models.DTO;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Admin
{
    public class AdminOverviewBase : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
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
            int currentUserId = await GetUserInfo();

            if (userId == currentUserId)
            {
                Console.WriteLine("You cannot delete your own user.");
                return;
            }

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

        protected void EditUser(int userId)
        {
            NavigationManager.NavigateTo($"/Admin/EditUser/{userId}");
        }
    }
}
