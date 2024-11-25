using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Headers;

namespace Scheduly.WebApp.Pages.Booking
{
    public class CreateCategoryBase : ComponentBase
    {
        [Inject]
        private ISnackbar Snackbar { get; set; }

        public string CategoryName { get; set; }

        public async Task CreateNewCategory()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                        var url = "https://localhost:7171/api/ResourceCategories/CreateResourceCategory";

                        // Create form data
                        var formData = new Dictionary<string, string>
                        {
                            { "name", CategoryName }
                        };

                        // Convert form data to string
                        var content = new FormUrlEncodedContent(formData);

                        // Send POST request
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
                            Snackbar.Add("Failed to create new resource category!", Severity.Error);

                            Console.WriteLine($"Failed to create new resource category. Status: {response.StatusCode}");

                            CategoryName = string.Empty;
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                        Snackbar.Add("Error creating new resource category!", Severity.Error);

                        Console.WriteLine($"An error occurred while making the request: {e.Message}");

                        CategoryName = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Error creating new resource category!", Severity.Error);

                Console.WriteLine($"Error creating new resource category: {ex.Message}");

                CategoryName = string.Empty;
            }

        }
    }
}
