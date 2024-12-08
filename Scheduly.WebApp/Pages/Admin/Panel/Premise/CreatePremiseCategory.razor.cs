using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Headers;

namespace Scheduly.WebApp.Pages.Admin.Panel.Premise
{
    public class CreatePremiseCategoryBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        public string CategoryName { get; set; }

        public async Task CreateNewCategory()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                        var url = "https://localhost:7171/api/PremiseCategories/CreatePremiseCategory";

                        var formData = new Dictionary<string, string>
                        {
                            { "name", CategoryName },
                            { "userId", userId.ToString() }
                        };

                        var content = new FormUrlEncodedContent(formData);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Created new category.", Severity.Success);

                            Console.WriteLine("Created new category.");

                            CategoryName = string.Empty;
                        }
                        else
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Failed to create new premise category!", Severity.Error);

                            Console.WriteLine($"Failed to create new premise category. Status: {response.StatusCode}");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"An error occurred while making the request: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Error creating new premise category!", Severity.Error);

                Console.WriteLine($"Error creating new premise category: {ex.Message}");
            }
        }
    }
}
