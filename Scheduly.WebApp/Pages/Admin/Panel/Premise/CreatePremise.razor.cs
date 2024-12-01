using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApi.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Scheduly.WebApp.Models;
using System;

namespace Scheduly.WebApp.Pages.Admin.Panel.Premise
{
    public class CreatePremiseBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }

        protected string PremiseName { get; set; }
        protected string PremiseSize { get; set; }
        protected string PremiseDescription { get; set; }
        protected bool MustBeApproved { get; set; }

        protected List<PremiseCategory> PremiseCategories;
        public string[] PremiseCategoryNames { get; set; }
        protected int _selectedPremiseCategoryId;

        protected class PremiseCategory
        {
            public int PremiseCategoryId { get; set; }
            public string Name { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadPremiseCategoryNames();
            _selectedPremiseCategoryId = PremiseCategories.FirstOrDefault()?.PremiseCategoryId ?? 0;
        }

        private async Task LoadPremiseCategoryNames()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = "https://localhost:7171/api/PremiseCategories";

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        PremiseCategories = await response.Content.ReadFromJsonAsync<List<PremiseCategory>>();
                        PremiseCategoryNames = PremiseCategories.Select(at => at.Name).ToArray();
                    }
                    else
                    {
                        Snackbar.Add("Failed to load premise category names.", Severity.Error);
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
                var premiseDto = new CreatePremiseDTO
                {
                    PremiseCategoryId = _selectedPremiseCategoryId,
                    Name = PremiseName,
                    Size = PremiseSize,
                    Description = PremiseDescription ?? string.Empty,
                    MustBeApproved = MustBeApproved
                };

                await SendResourceCreationRequest(premiseDto);
            }
            else
            {
                Snackbar.Add("Invalid input data.", Severity.Error);
            }
        }

        private bool ValidateResourceInputs()
        {
            return _selectedPremiseCategoryId > 0 && !string.IsNullOrEmpty(PremiseName) && !string.IsNullOrEmpty(PremiseSize);
        }

        private async Task SendResourceCreationRequest(CreatePremiseDTO premiseDto)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = "https://localhost:7171/api/Premises/CreatePremise";

                    var content = new StringContent(JsonSerializer.Serialize(premiseDto), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Resource created successfully.", Severity.Success);

                        PremiseName = string.Empty;
                        PremiseSize = string.Empty;
                        PremiseDescription = string.Empty;
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
