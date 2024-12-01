using Microsoft.AspNetCore.Components;
using MudBlazor;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;
using System.Text;

namespace Scheduly.WebApp.Pages.Admin
{
    public class CreateAbsenceTypeBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }

        public string Name { get; set; }
        public decimal WageFactor { get; set; }
        public bool MustBeApproved { get; set; }

        public async Task CreateAbsenceType()
        {
        //    if (ValidateAbsenceInputs())
        //    {
        //        var resourceDto = new AbsenceTypeDTO
        //        {
        //            AbsenceTypeId = AbsenceTypeId,
        //            Name = Name,
        //            WageFactor = WageFactor,
        //            MustBeApproved = MustBeApproved
        //        };

        //        await SendResourceCreationRequest(resourceDto);
        //    }
        //    else
        //    {
        //        Snackbar.Add("Invalid input data.", Severity.Error);
        //    }
        }

        //private bool ValidateAbsenceInputs()
        //{
        //    return AbsenceTypeId > 0 && !string.IsNullOrEmpty(Name);
        //}

        //private async Task SendResourceCreationRequest(CreateResourceDTO resourceDto)
        //{
        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            var url = "https://localhost:7171/api/Resources/CreateResource";

        //            var content = new StringContent(JsonSerializer.Serialize(resourceDto), Encoding.UTF8, "application/json");

        //            var response = await httpClient.PostAsync(url, content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                Snackbar.Add("Resource created successfully.", Severity.Success);

        //                ResourceName = string.Empty;
        //                ResourceAmount = 0;
        //                ResourceDescription = string.Empty;
        //                MustBeApproved = false;
        //            }
        //            else
        //            {
        //                Snackbar.Add("Failed to create resource.", Severity.Error);
        //            }
        //        }
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
        //    }
        //}
    }
}
