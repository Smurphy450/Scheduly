using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Scheduly.WebApp.Utilities
{
    public static class UserInfoHelper
    {
        public static async Task<int> GetUserIdAsync(AuthenticationStateProvider authStateProvider)
        {
            try
            {
                var authState = await authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    return int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : 0;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
                return 0;
            }
        }
    }
}
