using Humanizer.Localisation;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;

namespace Scheduly.WebApp.Pages.Booking
{
	public class BookingBase : ComponentBase
    {
        public List<Resource> ResourcesList = [];

        protected override async Task OnInitializedAsync()
		{
			try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var getAllResponse = await httpClient.GetAsync("https://localhost:7171/api/Resources");
                        if (getAllResponse.IsSuccessStatusCode)
                        {
                            var content = await getAllResponse.Content.ReadAsStringAsync();

                            var resources = JsonConvert.DeserializeObject<List<Resource>>(content);
                            ResourcesList = resources ?? new List<Resource>();

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving resources: {ex.Message}");
            }
        }
	}
}
