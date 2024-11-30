using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Profile
{
    public class ChangePasswordBase : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        public int UserId { get; set; }
        protected string CurrentPassword { get; set; }
        protected string NewPassword { get; set; }
        protected string NewPasswordReentered { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUserId();
        }

        private async Task GetUserId()
        {
            try
            {
                var authState = await authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    UserId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int a) ? a : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
            }
        }

        protected async Task SaveChanges()
        {
            if (NewPassword != NewPasswordReentered)
            {
                Console.WriteLine("New passwords do not match.");
                return;
            }

            var changePasswordDTO = new ChangePasswordDTO
            {
                UserId = UserId,
                OldPasswordHash = PasswordHasher.HashPassword(CurrentPassword),
                NewPasswordHash = PasswordHasher.HashPassword(NewPassword)
            };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsJsonAsync("https://localhost:7171/api/Users/ChangePassword", changePasswordDTO);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Password changed successfully.");
                        NavigationManager.NavigateTo("/Profile");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to change password. Status: {response.StatusCode}");
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
