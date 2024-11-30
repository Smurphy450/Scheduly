using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Profile
{
    public class ChangePasswordBase : ComponentBase
	{
		[Inject] NavigationManager NavigationManager { get; set; }
		[Inject] private AuthenticationStateProvider authStateProvider { get; set; }

		public int UserId { get; set; }
		protected string CurrentPassword { get; set; }
		protected string NewPassword { get; set; }
		protected string NewPasswordReentered { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await GetUserId();
		}

		private async Task GetUserId()
		{
			try
			{
				var authState = await authStateProvider.GetAuthenticationStateAsync();
				var user = authState.User;

				if (user.Identity.IsAuthenticated)
				{
					UserId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int a) ? a : 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error authenticating user: {ex.Message}");
			}
		}

		protected async Task SaveChanges()
		{

			NavigationManager.NavigateTo("/Profile");
		}
	}
}
