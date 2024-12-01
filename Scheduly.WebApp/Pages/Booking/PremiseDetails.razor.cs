using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Booking
{
    public class PremiseDetailsBase : ComponentBase
    {
        [Parameter] public int PremiseCategoryId { get; set; }

        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        protected DateTime? startDate { get; set; } = DateTime.Now.Date;
        protected TimeSpan? startTime { get; set; } = new TimeSpan(8, 0, 0);
        protected DateTime? endDate { get; set; } = DateTime.Now.Date;
        protected TimeSpan? endTime { get; set; } = new TimeSpan(16, 0, 0);

        public List<Premise> PremiseList = [];
        public string PremiseCategoryName = "";

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
                    await GetPremiseCategoryName(httpClient);

                    await GetAllPremises(httpClient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving premises: {ex.Message}");
            }
        }

        private async Task GetPremiseCategoryName(HttpClient httpClient)
        {
            try
            {
                var getNameResponse = await httpClient.GetAsync($"https://localhost:7171/api/PremiseCategories/{PremiseCategoryId}");
                if (getNameResponse.IsSuccessStatusCode)
                {
                    var content = await getNameResponse.Content.ReadAsStringAsync();

                    var premiseCategory = JsonConvert.DeserializeObject<PremiseCategory>(content);
                    PremiseCategoryName = premiseCategory?.Name ?? "";

                    Console.WriteLine("Retrieved premise category name.");
                }
                else
                {
                    Console.WriteLine($"Failed to get premise category name. Status: {getNameResponse.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        private async Task GetAllPremises(HttpClient httpClient)
        {
            try
            {
                var getAllResponse = await httpClient.GetAsync($"https://localhost:7171/api/Premises/Category/{PremiseCategoryId}");
                if (getAllResponse.IsSuccessStatusCode)
                {
                    var content = await getAllResponse.Content.ReadAsStringAsync();

                    var premise = JsonConvert.DeserializeObject<List<Premise>>(content);
                    PremiseList = premise ?? new List<Premise>();

                    Console.WriteLine("Retrieved all premises.");
                }
                else
                {
                    Console.WriteLine($"Failed to get all premises. Status: {getAllResponse.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while making the request: {e.Message}");
            }
        }

        protected async Task BookPremise(int premiseId)
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
                                PremiseId = premiseId,
                                ResourceId = null,
                                Start = CombineDateTimeAndTimeSpan(startDate, startTime),
                                End = CombineDateTimeAndTimeSpan(endDate, endTime),
                                Approved = false // TODO: Skal hentes fra PremisCatagory.
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
                        else
                        {
                            Snackbar.Add("Please select valid start and end dates and times.", Severity.Error);
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

        protected async Task DeletePremise(int premiseId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var getAllResponse = await httpClient.DeleteAsync($"https://localhost:7171/api/Premises/{premiseId}");
                    if (getAllResponse.IsSuccessStatusCode)
                    {
                        // Load items again
                        PremiseList.Clear();
                        await GetNameAndResources();

                        Console.WriteLine("Deleted premise.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to Deleted premise. Status: {getAllResponse.StatusCode}");
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
