using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;
using System.Text;

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
                        var content = new StringContent(JsonConvert.SerializeObject(CategoryName), Encoding.UTF8, "application/json");

                        var postResponse = await httpClient.PostAsync("https://localhost:7171/api/ResourceCategories", content);
                        if (postResponse.IsSuccessStatusCode)
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

                            Console.WriteLine($"Failed to create new resource category. Status: {postResponse.StatusCode}");

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
