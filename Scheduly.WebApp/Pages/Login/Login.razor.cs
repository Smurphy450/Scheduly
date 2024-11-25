using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Extensions;
using Scheduly.WebApi.Models;
using Scheduly.WebApp.Authentication;
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

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                var username = model.Username;
                var passwordHash = HashPassword(model.PasswordHash);

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
                            // Handle successful authentication
                            Snackbar.Add("Login successful!", Severity.Success);


                            NavigationManager.NavigateTo("/test");
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
