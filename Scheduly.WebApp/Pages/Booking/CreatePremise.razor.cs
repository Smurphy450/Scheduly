﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApi.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Scheduly.WebApp.Models;
using System;

namespace Scheduly.WebApp.Pages.Booking
{
    public class CreatePremiseBase : ComponentBase
    {
        [Parameter] public int PremiseCategoryId { get; set; }

        [Inject] private ISnackbar Snackbar { get; set; }

        public string PremiseName { get; set; }
        public string PremiseSize { get; set; }
        public string PremiseDescription { get; set; }
        public bool MustBeApproved { get; set; }

        public async Task CreateNewResource()
        {
            if (ValidateResourceInputs())
            {
                var premiseDto = new CreatePremiseDTO
                {
                    PremiseCategoryId = PremiseCategoryId,
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
            return PremiseCategoryId > 0 && !string.IsNullOrEmpty(PremiseName) && !string.IsNullOrEmpty(PremiseSize);
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

        //public async Task CreateNewResource()
        //{
        //    try
        //    {
        //        // TODO: Make this API work
        //        using (var httpClient = new HttpClient())
        //        {
        //            try
        //            {
        //                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                var url = "https://localhost:7171/api/Premises/CreatePremise";

        //                var requestBody = new
        //                {
        //                    premisecategoryid = PremiseCategoryId,
        //                    name = PremiseName,
        //                    size = PremiseSize,
        //                    description = PremiseDescription,
        //                    mustbeapproved = MustBeApproved
        //                };

        //                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        //                var response = await httpClient.PostAsync(url, content);

        //                if (response.IsSuccessStatusCode)
        //                {
        //                    Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        //                    Snackbar.Add("Created new premise.", Severity.Success);

        //                    Console.WriteLine("Created new premise.");

        //                    PremiseName = string.Empty;
        //                    PremiseSize = string.Empty;
        //                    PremiseDescription = string.Empty;
        //                    MustBeApproved = false;
        //                }
        //                else
        //                {
        //                    Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        //                    Snackbar.Add("Failed to create new premise!", Severity.Error);

        //                    Console.WriteLine($"Failed to create new premise. Status: {response.StatusCode}");
        //                }
        //            }
        //            catch (HttpRequestException e)
        //            {
        //                Console.WriteLine($"An error occurred while making the request: {e.Message}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        //        Snackbar.Add("Error creating new premise!", Severity.Error);

        //        Console.WriteLine($"Error creating new premise: {ex.Message}");
        //    }
        //}
    }
}
