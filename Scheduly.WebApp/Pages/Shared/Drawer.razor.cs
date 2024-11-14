using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Scheduly.WebApp.Pages.Shared
{
    public class DrawerBase : ComponentBase
    {
		public bool _open = false;

		public void ToggleDrawer()
		{
			_open = !_open;
		}
	}
}
