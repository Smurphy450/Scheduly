using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.User;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Profile
{
    public class ChangePasswordBase : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
		[Inject] private IJSRuntime JSRuntime { get; set; }

		protected string CurrentPassword { get; set; }
        protected string NewPassword { get; set; }
        protected string NewPasswordReentered { get; set; }
        protected async Task SaveChanges()
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            if (NewPassword != NewPasswordReentered)
            {
                Snackbar.Add("New passwords do not match.", Severity.Error);
                return;
            }

            var changePasswordDTO = CreateChangePasswordDTO(userId);

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsJsonAsync("https://localhost:7171/api/Users/ChangePassword", changePasswordDTO);
                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Password changed successfully.", Severity.Success);
						await JSRuntime.InvokeVoidAsync("history.back");
					}
                    else
                    {
                        Snackbar.Add($"Failed to change password. Status: {response.StatusCode}", Severity.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }
        private ChangePasswordDTO CreateChangePasswordDTO(int userId)
        {
            return new ChangePasswordDTO
            {
                UserId = userId,
                OldPasswordHash = PasswordHasher.HashPassword(CurrentPassword),
                NewPasswordHash = PasswordHasher.HashPassword(NewPassword)
            };
        }
    }
}
