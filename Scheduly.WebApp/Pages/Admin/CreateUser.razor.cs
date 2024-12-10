using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO.User;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace Scheduly.WebApp.Pages.Admin
{
    public class CreateUserBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        public UserProfileDTO UserProfile { get; set; } = new UserProfileDTO();
        public string NewPassword { get; set; }

        protected async Task CreateUser()
        {
            NewPassword = GenerateNewPassword(8);
            UserProfile.Password = NewPassword; // Set the unhashed password
            UserProfile.PasswordHash = PasswordHasher.HashPassword(NewPassword);

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                    var response = await httpClient.PostAsJsonAsync($"https://localhost:7171/api/Profiles/UserProfile?userId={userId}", UserProfile);
                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("User profile created successfully.", Severity.Success);
                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Snackbar.Add("Failed to create user profile.", Severity.Error);
                        Console.WriteLine($"Failed to create user profile. Status: {response.StatusCode}, Response: {responseContent}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        private string GenerateNewPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder result = new StringBuilder();
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                RandomNumberGenerator.Fill(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                result.Append(validChars[(int)(num % (uint)validChars.Length)]);
            }

            return result.ToString();
        }
    }
}
