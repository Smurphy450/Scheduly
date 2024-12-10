using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Premise;
using Scheduly.WebApp.Utilities;
using System;
using System.Net.Http.Headers;

namespace Scheduly.WebApp.Pages.Admin.Panel.Premise
{
    public class CreatePremiseCategoryBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private SchedulyContext _context { get; set; }
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

                        var premiseCategoryDTO = CreatePremiseCategoryDTO(CategoryName, userId);

                        var response = await SendCreateCategoryRequest(premiseCategoryDTO);

                        if (response.IsSuccessStatusCode)
                        {
                            ShowSnackbar("Created new category.", Severity.Success);

                            CategoryName = string.Empty;
                        }
                        else
                        {
                            ShowSnackbar("Failed to create new premise category!", Severity.Error);
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        ShowSnackbar("Failed to create new premise category!", Severity.Error);
                        await ErrorLoggingHelper.LogErrorAsync(_context, "CreateNewCategory", e);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSnackbar("Error creating new premise category!", Severity.Error);
                await LogErrorAsync(ex, "CreateNewCategory");
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

        private async Task<HttpResponseMessage> SendCreateCategoryRequest(CreatePremiseCategoryDTO premiseCategoryDTO)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = "https://localhost:7171/api/PremiseCategories/CreatePremiseCategory";
                    return await httpClient.PostAsJsonAsync(url, premiseCategoryDTO);
                }
                catch (HttpRequestException e)
                {
                    await LogErrorAsync(e, "SendCreateCategoryRequest");
                    throw;
                }
            }   
        }

        private void ShowSnackbar(string message, Severity severity)
        {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add(message, severity);
        }

        private async Task LogErrorAsync(Exception ex, string action)
        {
            var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
            await ErrorLoggingHelper.LogErrorAsync(_context, action, ex);
        }
    }
}
