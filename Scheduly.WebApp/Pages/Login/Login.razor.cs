using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace Scheduly.WebApp.Pages.Login
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        private HttpClient Http {  get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }
        //LoginModel model = new LoginModel();

        protected WebApi.Models.User model = new WebApi.Models.User();
        //protected class LoginModel
        //{
        //    public string Username { get; set; }
        //    public string Password { get; set; }
        //}

        protected async void HandleValidSubmit()
        {
            try
            {
                // skal lige kalde den spcifike GET rigtig, men her er et eks.
                var result = Http.PostAsJsonAsync<WebApi.Models.User>("api/user", model).Result;
                if (result.IsSuccessStatusCode)
                {
                    Snackbar.Add("Login successful!", Severity.Success);
                    Console.WriteLine($"Login successful for username: {model.Username}");
                    model = new WebApi.Models.User();
                }
                else
                {
                    Snackbar.Add("Login failed. Please check your credentials.", Severity.Error);
                    Console.WriteLine($"Login failed for username: {model.Username}");
                }
                //await Http.PostAsJsonAsync<WebApi.Models.User>("https://localhost:7171/api/user", model);
                //// Implement login logic here
                //Console.WriteLine($"Login attempt with username: {model.Username}, password: {model.PasswordHash}");

                // Clear form after submission
                //model = new WebApi.Models.User();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
                Console.WriteLine($"Error during login attempt: {ex.Message}");
            }
        }
    }
}
