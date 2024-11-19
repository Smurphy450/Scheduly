using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Scheduly.WebApp.Pages.Login
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        private ISnackbar Snackbar { get; set; }

        protected WebApi.Models.User model = new WebApi.Models.User();

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
                        // Handle successful authentication
                        Snackbar.Add("Login successful!", Severity.Success);
                        Console.WriteLine($"Login successful for username: {username}");

                        model = new WebApi.Models.User();
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
