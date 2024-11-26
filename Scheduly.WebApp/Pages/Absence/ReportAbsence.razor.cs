﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net.Http.Headers;
using System.Security.Claims;
using Scheduly.WebApp.Models;
using Scheduly.WebApi.Models;
using static System.Net.WebRequestMethods;

namespace Scheduly.WebApp.Pages.Absence
{
    public class ReportAbsenceBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }

        public int AbsenceTypeId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset Start { get; set; } = DateTimeOffset.Now; // TODO: Get current date, where time is set to 8 AM
        public DateTimeOffset End { get; set; } = DateTimeOffset.Now; // TODO: Get current date, where time is set to 4 PM
        public DateTime? DatetimeStart { get; set; } = DateTime.Today;
        public DateTime? DatetimeEnd { get; set; } = DateTime.Today;
        public string Description { get; set; }
        public string[] AbsenceTypeNames { get; set; }
        public TimeSpan? StartTime { get; set; } = new TimeSpan(8, 0, 0);
        public TimeSpan? EndTime { get; set; } = new TimeSpan(16, 0, 0);

        protected List<AbsenceType> AbsenceTypes;
        protected int _selectedAbsenceTypeId;

        protected class AbsenceType
        {
            public int AbsenceTypeId { get; set; }
            public string Name { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetUserId();
            await LoadAbsenceTypeNames();
            _selectedAbsenceTypeId = AbsenceTypes.FirstOrDefault()?.AbsenceTypeId ?? 0;
        }

        protected async Task ReportAbsenceAsync()
        {
            await GetUserId();

            if (ValidateInputs())
            {
                var absenceDto = new ReportAbsenceDTO
                {
                    AbsenceTypeId = _selectedAbsenceTypeId,
                    UserId = UserId,
                    DatetimeStart = CombineDateTimeAndTimeSpan(DatetimeStart, StartTime),
                    DatetimeEnd = CombineDateTimeAndTimeSpan(DatetimeEnd, EndTime),
                    Description = Description
                };

                await SendAbsenceReport(absenceDto);
            }
            else
            {
                Snackbar.Add("Invalid input data.", Severity.Error);
            }
        }

        private bool ValidateInputs()
        {
            return _selectedAbsenceTypeId > 0 && UserId > 0 && DatetimeStart.HasValue && DatetimeEnd.HasValue && !string.IsNullOrEmpty(Description) && StartTime.HasValue && EndTime.HasValue;
        }

        private async Task SendAbsenceReport(ReportAbsenceDTO absenceDto)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = "https://localhost:7171/api/Absence/Report";

                    var response = await httpClient.PostAsJsonAsync(url, absenceDto);

                    if (response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Absence reported successfully.", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add("Failed to report absence.", Severity.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
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

        private async Task LoadAbsenceTypeNames()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = "https://localhost:7171/api/AbsenceTypes";

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        AbsenceTypes = await response.Content.ReadFromJsonAsync<List<AbsenceType>>();
                        AbsenceTypeNames = AbsenceTypes.Select(at => at.Name).ToArray();
                        Snackbar.Add("Absence type names loaded successfully.", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add("Failed to load absence type names.", Severity.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Snackbar.Add($"An error occurred while making the request: {e.Message}", Severity.Error);
            }
        }

        private DateTimeOffset CombineDateTimeAndTimeSpan(DateTime? date, TimeSpan? time)
        {
            if (date.HasValue && time.HasValue)
            {
                return new DateTimeOffset(date.Value.Date + time.Value);
            }
            throw new ArgumentException("Date or Time is null");
        }
    }
}
