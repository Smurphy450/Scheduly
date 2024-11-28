using Microsoft.AspNetCore.Components;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApi.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Scheduly.WebApp.Pages.Booking
{
    public class CreateResourceBase : ComponentBase
    {
        [Parameter] public int CategoryId { get; set; }

        [Inject] private ISnackbar Snackbar { get; set; }

        public string ResourceName { get; set; }
        public int ResourceAmount { get; set; }
        public string ResourceDescription { get; set; }
        public bool MustBeApproved { get; set; }

        public async Task CreateNewResource()
        {
            if (ValidateResourceInputs())
            {
                var resourceDto = new CreateResourceDTO
                {
                    CategoryId = CategoryId,
                    Name = ResourceName,
                    Amount = ResourceAmount,
                    Description = ResourceDescription ?? string.Empty,
                    MustBeApproved = MustBeApproved
                };

                await SendResourceCreationRequest(resourceDto);
            }
            else
            {
                Snackbar.Add("Invalid input data.", Severity.Error);
            }
        }

        private bool ValidateResourceInputs()
        {
            return CategoryId > 0 && !string.IsNullOrEmpty(ResourceName) && ResourceAmount > 0;
        }

        private async Task SendResourceCreationRequest(CreateResourceDTO resourceDto)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = "https://localhost:7171/api/Resources/CreateResource";

                    var content = new StringContent(JsonSerializer.Serialize(resourceDto), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Resource created successfully.", Severity.Success);

                        ResourceName = string.Empty;
                        ResourceAmount = 0;
                        ResourceDescription = string.Empty;
                        MustBeApproved = false;
                    }
                    else
                    {
                        Snackbar.Add("Failed to create resource.", Severity.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
            }
        }
    }
}
