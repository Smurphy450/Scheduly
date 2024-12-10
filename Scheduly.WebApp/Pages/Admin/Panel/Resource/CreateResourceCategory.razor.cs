using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO.Resource;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Headers;

namespace Scheduly.WebApp.Pages.Admin.Panel.Resource
{
    public class CreateResourceCategoryBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        public string CategoryName { get; set; }

        public async Task CreateNewCategory()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        int userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                        var createResourceCategoryDTO = CreateResourceCategoryDTO(userId);

                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var url = "https://localhost:7171/api/ResourceCategories/CreateResourceCategory";

                        var response = await httpClient.PostAsJsonAsync(url, createResourceCategoryDTO);

                        if (response.IsSuccessStatusCode)
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Created new category.", Severity.Success);

                            Console.WriteLine("Created new category.");

                            CategoryName = string.Empty;
                        }
                        else
                        {
                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                            Snackbar.Add("Failed to create new resource category!", Severity.Error);

                            Console.WriteLine($"Failed to create new resource category. Status: {response.StatusCode}");
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
                Snackbar.Add("Error creating new resource category!", Severity.Error);

                Console.WriteLine($"Error creating new resource category: {ex.Message}");
            }
        }
        private CreateResourceCategoryDTO CreateResourceCategoryDTO(int userId)
        {
            return new CreateResourceCategoryDTO
            {
                Name = CategoryName,
                UserId = userId
            };
        }
    }
}
