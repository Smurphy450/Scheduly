using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;

namespace Scheduly.WebApp.Pages.Booking
{
    public class BookingDetailsBase : ComponentBase
    {
        [Parameter] public int CategoryId { get; set; }

        public List<Resource> ResourceList = [];
        public string ResourceCategoryName = "";

        protected override async Task OnInitializedAsync()
        {
            await GetNameAndResources();
        }

        private async Task GetNameAndResources()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    await GetResourceCategoryName(httpClient);

                    await GetAllResources(httpClient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving resources: {ex.Message}");
            }
        }

        private async Task GetResourceCategoryName(HttpClient httpClient)
        {
            try
            {
                var getNameResponse = await httpClient.GetAsync($"https://localhost:7171/api/ResourceCategories/{CategoryId}");
                if (getNameResponse.IsSuccessStatusCode)
                {
                    var content = await getNameResponse.Content.ReadAsStringAsync();

                    var resourceCategory = JsonConvert.DeserializeObject<ResourceCategory>(content);
                    ResourceCategoryName = resourceCategory?.Name ?? "";

                    Console.WriteLine("Retrieved resource category name.");
                }
                else
                {
                    Console.WriteLine($"Failed to get resource category name. Status: {getNameResponse.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        private async Task GetAllResources(HttpClient httpClient)
        {
            try
            {
                var getAllResponse = await httpClient.GetAsync($"https://localhost:7171/api/Resources/Category/{CategoryId}");
                if (getAllResponse.IsSuccessStatusCode)
                {
                    var content = await getAllResponse.Content.ReadAsStringAsync();

                    var resource = JsonConvert.DeserializeObject<List<Resource>>(content);
                    ResourceList = resource ?? new List<Resource>();

                    Console.WriteLine("Retrieved all resources.");
                }
                else
                {
                    Console.WriteLine($"Failed to get all resources. Status: {getAllResponse.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }
    }
}
