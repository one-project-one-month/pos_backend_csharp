using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace DotNet8.PosFrontendBlazor.Pages.Staff
{
    public partial class P_StaffDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private StaffRequestModel reqModel = new();

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            reqModel.StaffCode = "";
            DateTime StaffDOB = reqModel.DateOfBirth.ToDateTime();
            DateTime DateTimeNow = DateTime.Now;
            TimeSpan ageSpan = DateTimeNow - StaffDOB;
            int StaffAge = (int)(ageSpan.Days / 365.25);
            if (StaffAge < 18)
            {
                //await JSRuntime.InvokeVoidAsync("alert", "Staff Age must be greater than 18 years.");
                InjectService.ShowMessage("Staff Age must be greater than 18 years.", EnumResponseType.Success);
            }
            else
            {
                var response = await HttpClientService.ExecuteAsync<StaffResponseModel>(
                Endpoints.Staff,
                EnumHttpMethod.Post,
                reqModel
            );
                if (response.IsError)
                {
                    Console.WriteLine(response.Message);
                    Console.WriteLine(EnumResponseType.Error);
                    InjectService.ShowMessage(response.Message, EnumResponseType.Error);
                    return;
                }

                InjectService.ShowMessage(response.Message, EnumResponseType.Success);
                MudDialog.Close();
            }

        }
    }
}