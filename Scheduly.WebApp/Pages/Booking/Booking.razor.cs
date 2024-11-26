using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;

namespace Scheduly.WebApp.Pages.Booking
{
	public class BookingBase : ComponentBase
    {
        public List<ResourceCategory> ResourceCategoryList = [];
        public List<PremiseCategory> PremiseCategoryList = [];

        protected override async Task OnInitializedAsync()
		{
            await GetAllResourceTypes();
            await GetAllPremiseTypes();
        }

        private async Task GetAllResourceTypes()
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
        private async Task GetAllPremiseTypes()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var getAllResponse = await httpClient.GetAsync("https://localhost:7171/api/PremiseCategories");
                        if (getAllResponse.IsSuccessStatusCode)
                        {
                            var content = await getAllResponse.Content.ReadAsStringAsync();

                            var premiseCategories = JsonConvert.DeserializeObject<List<PremiseCategory>>(content);
                            PremiseCategoryList = premiseCategories ?? new List<PremiseCategory>();

                            Console.WriteLine("Retrieved all premise categories.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get all premise categories. Status: {getAllResponse.StatusCode}");
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
                Console.WriteLine($"Error retrieving premise categories: {ex.Message}");
            }
        }

        protected async Task DeleteResourceCategory(int resourceCategoryId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var getAllResponse = await httpClient.DeleteAsync($"https://localhost:7171/api/ResourceCategories/{resourceCategoryId}");
                    if (getAllResponse.IsSuccessStatusCode)
                    {
                        // Load items again
                        ResourceCategoryList.Clear();
                        await GetAllResourceTypes();

                        Console.WriteLine("Deleted category.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to deleted category. Status: {getAllResponse.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        protected async Task DeletePremiseCategory(int premiseCategoryId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var getAllResponse = await httpClient.DeleteAsync($"https://localhost:7171/api/PremiseCategories/{premiseCategoryId}");
                    if (getAllResponse.IsSuccessStatusCode)
                    {
                        // Load items again
                        PremiseCategoryList.Clear();
                        await GetAllPremiseTypes();

                        Console.WriteLine("Deleted category.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to deleted category. Status: {getAllResponse.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }
    }
}
