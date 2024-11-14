using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace Scheduly.WebApp.Pages.Login
{
    public class LoginBase : ComponentBase
    {
        protected LoginModel model = new LoginModel();

        protected class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        protected void HandleValidSubmit()
        {
            // Implement login logic here
            Console.WriteLine($"Login attempt with username: {model.Username}, password: {model.Password}");

            // Clear form after submission
            model = new LoginModel();
        }
    }
}
