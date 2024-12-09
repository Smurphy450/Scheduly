using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Headers;

namespace Scheduly.WebApp.Pages.Admin.Panel.Premise
{
    public class CreatePremiseCategoryBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        public string CategoryName { get; set; }

        public async Task CreateNewCategory()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var url = "https://localhost:7171/api/PremiseCategories/CreatePremiseCategory";

                        var premiseCategoryDTO = CreatePremiseCategoryDTO(CategoryName, userId);

                        var response = await httpClient.PostAsJsonAsync(url, premiseCategoryDTO);

                        if (response.IsSuccessStatusCode)
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Created new category.", Severity.Success);

                            CategoryName = string.Empty;
                        }
                        else
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Failed to create new premise category!", Severity.Error);
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"An error occurred while making the request: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Error creating new premise category!", Severity.Error);
            }
        }

        private CreatePremiseCategoryDTO CreatePremiseCategoryDTO(string name, int userId)
        {
            return new CreatePremiseCategoryDTO
            {
                Name = name,
                UserId = userId
            };
        }
    }
}
