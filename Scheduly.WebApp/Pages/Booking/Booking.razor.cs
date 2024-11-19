using Microsoft.AspNetCore.Components;

namespace Scheduly.WebApp.Pages.Booking
{
	public class BookingBase : ComponentBase
    {
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
