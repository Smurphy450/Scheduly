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

namespace Scheduly.WebApp.Pages.Login
{
    public class LoginBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected User model = new User();
        [Inject] private IJSRuntime JSRuntime { get; set; }
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

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                    var url = "https://localhost:7171/api/Users/authenticate";

                    // Create form data
                    var formData = new Dictionary<string, string>
                    {
                        { "username", username },
                        { "passwordHash", passwordHash }
                    };

                    // Convert form data to string
                    var content = new FormUrlEncodedContent(formData);

                    // Send POST request
                    var response = await httpClient.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var contentString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error Details: {contentString}");
                        Snackbar.Add("Authentication failed. Please check your credentials.", Severity.Error);
                        Console.WriteLine($"Authentication failed for username: {username}");
                    }
                    else
                    {
                        var userAuthResponse = await response.Content.ReadFromJsonAsync<UserSession>();
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
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An error occurred during authentication: {ex.Message}", Severity.Error);
                Console.WriteLine($"Error during authentication attempt: {ex.Message}");
            }
        }
    }
}
