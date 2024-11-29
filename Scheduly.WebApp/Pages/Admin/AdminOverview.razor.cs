using Microsoft.AspNetCore.Components;

namespace Scheduly.WebApp.Pages.Admin
{
    public class AdminOverviewBase : ComponentBase
    {
        protected override void OnInitialized()
        {
            Console.WriteLine("Admin Overview Page Initialized.");
        }
    }
}
