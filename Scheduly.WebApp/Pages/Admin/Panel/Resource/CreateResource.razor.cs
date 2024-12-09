using Microsoft.AspNetCore.Components;
using MudBlazor;
using Scheduly.WebApi.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Scheduly.WebApi.Models.DTO.Resource;

namespace Scheduly.WebApp.Pages.Admin.Panel.Resource
{
    public class CreateResourceBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }

        public string ResourceName { get; set; }
        public int ResourceAmount { get; set; }
        public string ResourceDescription { get; set; }
        public bool MustBeApproved { get; set; }

        protected List<ResourceCategory> ResourceCategories;
        public string[] ResourceCategoryNames { get; set; }
        protected int _selectedResourceCategoryId;

        protected class ResourceCategory
        {
            public int CategoryId { get; set; }
            public string Name { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadResourceCategoryNames();
            _selectedResourceCategoryId = ResourceCategories.FirstOrDefault()?.CategoryId ?? 0;
        }

        private async Task LoadResourceCategoryNames()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = "https://localhost:7171/api/ResourceCategories";

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        ResourceCategories = await response.Content.ReadFromJsonAsync<List<ResourceCategory>>();
                        ResourceCategoryNames = ResourceCategories.Select(at => at.Name).ToArray();
                    }
                    else
                    {
                        Snackbar.Add("Failed to load resource category names.", Severity.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
            }
        }

        protected async Task CreateNewResource()
        {
            if (ValidateResourceInputs())
            {
                var resourceDto = new CreateResourceDTO
                {
                    CategoryId = _selectedResourceCategoryId,
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
            return _selectedResourceCategoryId > 0 && !string.IsNullOrEmpty(ResourceName) && ResourceAmount > 0;
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
