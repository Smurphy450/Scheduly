using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Booking;
using Scheduly.WebApp.Utilities;
using System.Net.Http;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Booking
{
    public class ResourceDetailsBase : ComponentBase
    {
        [Parameter] public int CategoryId { get; set; }

        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        protected DateTime? startDate { get; set; } = DateTime.Now.Date;
        protected TimeSpan? startTime { get; set; } = new TimeSpan(8, 0, 0);
        protected DateTime? endDate { get; set; } = DateTime.Now.Date;
        protected TimeSpan? endTime { get; set; } = new TimeSpan(16, 0, 0);

        public List<WebApi.Models.Resource> ResourceList = [];
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

                    var resource = JsonConvert.DeserializeObject<List<WebApi.Models.Resource>>(content);
                    ResourceList = resource ?? new List<WebApi.Models.Resource>();

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

        protected async Task BookResource(int premiseId)
        {
            try
            {
                int userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                if (userId > 0)
                {
                    using (var httpClient = new HttpClient())
                    {
                        if (startDate.HasValue && startTime.HasValue && endDate.HasValue && endTime.HasValue)
                        { 
                            var createBookingDTO = new CreateBookingDTO
                            {
                                UserId = userId,
                                PremiseId = null,
                                ResourceId = premiseId,
                                Start = CombineDateTimeAndTimeSpan(startDate, startTime),
                                End = CombineDateTimeAndTimeSpan(endDate, endTime),
                                Approved = false //TODO: Skal hentes fra PremisCatagory.
                            };

                            var content = new StringContent(JsonConvert.SerializeObject(createBookingDTO), System.Text.Encoding.UTF8, "application/json");
                            var response = await httpClient.PostAsync("https://localhost:7171/api/Bookings/CreateBooking", content);

                            if (response.IsSuccessStatusCode)
                            {
                                Snackbar.Add("Booking created successfully.", Severity.Success);
                                Console.WriteLine("Booking created successfully.");
                            }
                            else
                            {
                                Snackbar.Add("Failed to create booking.", Severity.Error);
                                Console.WriteLine($"Failed to create booking. Status: {response.StatusCode}");
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        private DateTimeOffset CombineDateTimeAndTimeSpan(DateTime? date, TimeSpan? time)
        {
            if (date.HasValue && time.HasValue)
            {
                return new DateTimeOffset(date.Value.Date + time.Value);
            }
            throw new ArgumentException("Date or Time is null");
        }

        protected async Task DeleteResource(int resourceId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    int userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                    var getAllResponse = await httpClient.DeleteAsync($"https://localhost:7171/api/Resources/{resourceId}?userId={userId}");
                    if (getAllResponse.IsSuccessStatusCode)
                    {
                        // Load items again
                        ResourceList.Clear();
                        await GetNameAndResources();

                        Console.WriteLine("Deleted resource.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to Deleted resource. Status: {getAllResponse.StatusCode}");
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
