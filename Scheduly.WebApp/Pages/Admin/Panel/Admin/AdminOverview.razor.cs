using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO.User;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Admin.Panel.Admin
{
    public class AdminOverviewBase : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
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
                    }
                    else
                    {
                        Snackbar.Add("Failed to get all users.", Severity.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
            }
        }
        protected async Task DeleteUser(int userId)
        {
            int currentUserId = await UserInfoHelper.GetUserIdAsync(authStateProvider);

            if (userId == currentUserId)
            {
                Snackbar.Add("You cannot delete your own user.", Severity.Error);
                return;
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync($"https://localhost:7171/api/Users/{userId}?userId={currentUserId}");
                    if (response.IsSuccessStatusCode)
                    {
                        UserList = UserList.Where(u => u.UserId != userId).ToList();
                        Snackbar.Add("User deleted successfully.", Severity.Error);
                    }
                    else
                    {
                        Snackbar.Add($"Failed to delete user. Status: {response.StatusCode}", Severity.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
            }
        }

        protected void EditUser(int userId)
        {
            NavigationManager.NavigateTo($"/Admin/EditUser/{userId}");
        }
    }
}
