using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Scheduly.WebApi.Models;
using System.Net.Http;

namespace Scheduly.WebApp.Pages.Booking
{
    public class PremiseDetailsBase : ComponentBase
    {
        [Parameter] public int PremiseCategoryId { get; set; }

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

        // TODO:
        protected async Task BookPremise()
        {
            //try
            //{
            //    var getAllResponse = await httpClient.GetAsync($"https://localhost:7171/api/Resources/Category/{CategoryId}");
            //    if (getAllResponse.IsSuccessStatusCode)
            //    {
            //        var content = await getAllResponse.Content.ReadAsStringAsync();

            //        var resource = JsonConvert.DeserializeObject<List<Resource>>(content);
            //        ResourceList = resource ?? new List<Resource>();

            //        Console.WriteLine("Retrieved all resources.");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Failed to get all resources. Status: {getAllResponse.StatusCode}");
            //    }
            //}
            //catch (HttpRequestException e)
            //{
            //    Console.WriteLine($"An error occurred while making the request: {e.Message}");
            //}
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
