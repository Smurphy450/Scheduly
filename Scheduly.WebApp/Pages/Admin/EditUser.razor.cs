using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO.Notification;
using Scheduly.WebApi.Models.DTO.User;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace Scheduly.WebApp.Pages.Admin
{
    public class EditUserBase : ComponentBase
    {
        [Parameter] public int UserId { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
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
                    int userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                    var response = await httpClient.PutAsJsonAsync($"https://localhost:7171/api/Profiles/UserProfile/{userId}", UserProfile);
                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("User profile updated successfully.", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add("Failed to update user profile.", Severity.Error);
                        Console.WriteLine($"Failed to update user profile. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        protected async Task GenerateNewPassword()
        {
            var newPassword = GenerateRandomPassword(8);
            NewPassword = newPassword;
            Console.WriteLine($"New password (unencrypted): {newPassword}");

            var notification = new CreateNotificationDTO
            {
                UserId = UserId,
                Sms = true,
                Email = true,
                Message = $"New password: '{newPassword}' for Scheduly"
            };

            bool notificationResult = await NotificationHelper.PostNotificationAsync(notification);

            if (notificationResult)
            {
                Console.WriteLine("Notification sent successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send notification.");
            }

            // Hash the password after sending the notification
            var hashedPassword = PasswordHasher.HashPassword(newPassword);

            var updatePasswordDTO = new UpdatePasswordDTO
            {
                UserId = UserId,
                PasswordHash = hashedPassword
            };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://localhost:7171/api/Users/UpdatePassword", updatePasswordDTO);
                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Password updated successfully.", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add("Failed to update password.", Severity.Error);
                        Console.WriteLine($"Failed to update password. Status: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
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
