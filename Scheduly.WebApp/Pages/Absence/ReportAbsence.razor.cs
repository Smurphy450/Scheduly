﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Scheduly.WebApp.Pages.Absence
{
    public class ReportAbsenceBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        public int AbsenceTypeId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public string Description { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUserId();
            await SaveAbsenceToDb();
        }

        private async Task SaveAbsenceToDb()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                        var url = "https://localhost:7171/api/ResourceCategories/CreateResourceCategory";

                        var formData = new Dictionary<string, string>
                        {
                            //{ "name", CategoryName }
                        };

                        var content = new FormUrlEncodedContent(formData);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Absence reported.", Severity.Success);

                            Console.WriteLine("Absence reported.");

                            //AbsenceTypeId = 0;
                            //Start = string.Empty;
                            //End = string.Empty;
                            //Description = string.Empty;
                        }
                        else
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Failed to report absence!", Severity.Error);

                            Console.WriteLine($"Failed to report absence. Status: {response.StatusCode}");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                        Snackbar.Add("Error reporting absence!", Severity.Error);

                        Console.WriteLine($"An error occurred while making the request: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Error reporting absence!", Severity.Error);

                Console.WriteLine($"Error reporting absence: {ex.Message}");
            }
        }

        private async Task GetUserId()
        {
            try
            {
                var authState = await authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    UserId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int a) ? a : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
            }
        }
    }
}