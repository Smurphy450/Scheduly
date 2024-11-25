using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Scheduly.WebApp.Pages.Absence
{
	public class AbsenceBase : ComponentBase
	{
        public int Index = -1; //default value cannot be 0 -> first selectedindex is 0.

        // TODO: Get absence type
        public double[] data = { 1, 2, 3, 4 };

        public string[] AbsenceArray = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // TODO: Get UserId from logged in user
                var userId = 2; // temp
                
                // TODO: Get amount of times the absence types have been used
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var getAllResponse = await httpClient.GetAsync($"https://localhost:7171/api/Absence/User/{userId}");
                        if (getAllResponse.IsSuccessStatusCode)
                        {
                            var content = await getAllResponse.Content.ReadAsStringAsync();

                            var absence = JsonConvert.DeserializeObject<List<Scheduly.WebApi.Models.Absence>>(content);
                            AbsenceArray = Array.ConvertAll(absence.ToArray(), x => x.ToString());

                            Console.WriteLine("Retrieved list of absence for user.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to get all absence for user. Status: {getAllResponse.StatusCode}");
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
                Console.WriteLine($"Error retrieving absence for user: {ex.Message}");
            }
        }
    }
}
