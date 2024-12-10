using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Extensions;
using Scheduly.WebApi.Models;
using Scheduly.WebApp.Authentication;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Scheduly.WebApp.Pages.Login
{
    public class LoginBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        protected User model = new User();
        protected async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await JSRuntime.InvokeVoidAsync("document.activeElement.blur");
                await HandleValidSubmit();
            }
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                var username = model.Username;
                var passwordHash = PasswordHasher.HashPassword(model.PasswordHash);

                var userAuthResponse = await AuthenticateUserAsync(username, passwordHash);

                if (userAuthResponse != null)
                {
                    var customerAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
                    await customerAuthStateProvider.UpdateAuthenticationState(new UserSession
                    {
                        Username = userAuthResponse.Username,
                        UserID = userAuthResponse.UserID,
                        Role = userAuthResponse.Role
                    });

                    NavigationManager.NavigateTo("/Overview");
                }
                else
                {
                    Snackbar.Add("Authentication failed. Please check your credentials.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An error occurred during authentication: {ex.Message}", Severity.Error);
                Console.WriteLine($"Error during authentication attempt: {ex.Message}");
            }
        }

        private async Task<UserSession> AuthenticateUserAsync(string username, string passwordHash)
        {
            using (var httpClient = new HttpClient())
            {
                var url = "https://localhost:7171/api/Users/authenticate";

                var userAuthRequest = new UserAuthRequest
                {
                    Username = username,
                    PasswordHash = passwordHash
                };

                var content = new StringContent(JsonSerializer.Serialize(userAuthRequest), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserSession>();
                }

                return null;
            }
        }
    }
}
