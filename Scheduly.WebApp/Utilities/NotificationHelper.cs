using Newtonsoft.Json;
using Scheduly.WebApi.Models.DTO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scheduly.WebApp.Utilities
{
    public static class NotificationHelper
    {
        public static async Task<bool> PostNotificationAsync(CreateNotificationDTO createNotificationDTO)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(createNotificationDTO), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://localhost:7171/api/Notifications", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Failed to create notification. Status: {response.StatusCode}, Error: {errorMessage}");
                        return false;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
                return false;
            }
        }
    }
}
