

namespace DotNet8.PosFrontendBlazor.Pages.Staff
{
    public partial class P_StaffDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private StaffRequestModel reqModel = new();

        [Parameter] public DateTime? DateOfBirth { get; set; }
        [Parameter] public int StaffId { get; set; }

        [Parameter] public string StaffCode { get; set; } = null!;

        [Parameter] public string StaffName { get; set; } = null!;

        [Parameter] public string MobileNo { get; set; } = null!;

        [Parameter] public string Gender { get; set; } = null!;

        [Parameter] public string StateCode { get; set; } = null!;

        [Parameter] public string TownshipCode { get; set; } = null!;

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            DateTime StaffDOB = Convert.ToDateTime(reqModel.DateOfBirth);
            DateTime DateTimeNow = DateTime.Now;
            TimeSpan ageSpan = DateTimeNow - StaffDOB;
            int StaffAge = (int)(ageSpan.Days / 365.25);
            int NoOfPhNo = reqModel.MobileNo.Length;
            
            if (StaffAge < 18)
            {
                //await JSRuntime.InvokeVoidAsync("alert", "Staff Age must be greater than 18 years.");
                InjectService.ShowMessage("Staff Age must be greater than 18 years.", EnumResponseType.Error);
            }
            else if (NoOfPhNo > 11 || NoOfPhNo < 11)
            {
                InjectService.ShowMessage("PhoneNo must have 11 digit.", EnumResponseType.Error);
            }
            else if (NoOfPhNo > 11 || NoOfPhNo < 11)
            {
                InjectService.ShowMessage("PhoneNo must have 11 digit.", EnumResponseType.Error);
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
                    //Console.WriteLine(response.Message);
                    //Console.WriteLine(EnumResponseType.Error);
                    InjectService.ShowMessage(response.Message, EnumResponseType.Error);
                    return;
                }

                InjectService.ShowMessage(response.Message, EnumResponseType.Success);
                MudDialog.Close();
            }
            
        }
    }
}