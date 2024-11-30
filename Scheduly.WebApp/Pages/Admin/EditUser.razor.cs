using Microsoft.AspNetCore.Components;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace Scheduly.WebApp.Pages.Admin
{
    public class EditUserBase : ComponentBase
    {
        [Parameter] public int UserId { get; set; }
        public UserProfileDTO UserProfile { get; set; } = new UserProfileDTO();
        public string NewPassword { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadUserProfile();
        }

        private async Task LoadUserProfile()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7171/api/Profiles/UserProfile/{UserId}");
                    if (response.IsSuccessStatusCode)
                    {
                        UserProfile = await response.Content.ReadFromJsonAsync<UserProfileDTO>();
                        Console.WriteLine("User profile loaded successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to load user profile. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        protected async Task SaveChanges()
        {
            if (!string.IsNullOrEmpty(NewPassword))
            {
                UserProfile.PasswordHash = PasswordHasher.HashPassword(NewPassword);
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsJsonAsync($"https://localhost:7171/api/Profiles/UserProfile/{UserId}", UserProfile);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("User profile updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to update user profile. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        protected void GenerateNewPassword()
        {
            var newPassword = GenerateRandomPassword(8);
            NewPassword = newPassword;
            Console.WriteLine($"New password (unencrypted): {newPassword}");
        }

        private string GenerateRandomPassword(int length)
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
