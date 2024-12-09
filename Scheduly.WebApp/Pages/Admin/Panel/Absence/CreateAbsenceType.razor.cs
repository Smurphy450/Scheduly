using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Scheduly.WebApi.Models.DTO.Absence;
using Scheduly.WebApp.Utilities;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Scheduly.WebApp.Pages.Admin.Panel.Absence
{
    public class CreateAbsenceTypeBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Parameter] public int Id { get; set; } = 0;
        public string Name { get; set; }
        public decimal WageFactor { get; set; } = 100;
        public bool MustBeApproved { get; set; }

        public async Task CreateAbsenceType()
        {
            if (ValidateAbsenceInputs())
            {
                var userId = await UserInfoHelper.GetUserIdAsync(authStateProvider);
                // Adjust WageFactor
                WageFactor = WageFactor / 100;

                var absenceTypeDto = new AbsenceTypeDTO
                {
                    AbsenceTypeId = Id, // ID is set to 0 for creation
                    Name = Name,
                    WageFactor = WageFactor,
                    MustBeApproved = MustBeApproved,
                    UserId = userId
                };

                await SendAbsenceTypeCreationRequest(absenceTypeDto);
            }
            else
            {
                Snackbar.Add("Invalid input data.", Severity.Error);
            }
        }

        private bool ValidateAbsenceInputs()
        {
            return !string.IsNullOrEmpty(Name);
        }

        private async Task SendAbsenceTypeCreationRequest(AbsenceTypeDTO absenceTypeDto)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = "https://localhost:7171/api/AbsenceTypes/Upsert";

                    var response = await httpClient.PostAsJsonAsync(url, absenceTypeDto);

                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Absence type created successfully.", Severity.Success);

                        // Reset form fields
                        Name = string.Empty;
                        WageFactor = 100;
                        MustBeApproved = false;
                    }
                    else
                    {
                        Snackbar.Add("Failed to create absence type.", Severity.Error);
                        WageFactor = WageFactor * 100;
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
