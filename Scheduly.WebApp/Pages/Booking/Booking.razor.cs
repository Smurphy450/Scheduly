using Humanizer.Localisation;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;

namespace Scheduly.WebApp.Pages.Booking
{
	public class BookingBase : ComponentBase
    {
        public List<ResourceCategory> ResourceCategoryList = [];

        protected override async Task OnInitializedAsync()
		{
			try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var getAllResponse = await httpClient.GetAsync("https://localhost:7171/api/ResourceCategories");
                        if (getAllResponse.IsSuccessStatusCode)
                        {
                            var content = await getAllResponse.Content.ReadAsStringAsync();

                            var resourceCategories = JsonConvert.DeserializeObject<List<ResourceCategory>>(content);
							ResourceCategoryList = resourceCategories ?? new List<ResourceCategory>();

                            Console.WriteLine("Retrieved all resource categories.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get all resource categories. Status: {getAllResponse.StatusCode}");
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
                Console.WriteLine($"Error retrieving resource categories: {ex.Message}");
            }
        }
	}
}
