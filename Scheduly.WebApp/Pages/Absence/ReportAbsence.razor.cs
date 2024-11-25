using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Scheduly.WebApp.Pages.Absence
{
    public class ReportAbsenceBase : ComponentBase
    {
        public int AbsenceTypeId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public string Description { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
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

                            //Console.WriteLine("Retrieved list of absence for user.");
                        }
                        else
                        {
                            //Console.WriteLine($"Failed to get all absence for user. Status: {getAllResponse.StatusCode}");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        //Console.WriteLine($"An error occurred while making the request: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error retrieving absence for user: {ex.Message}");
            }
        }
    }
}
