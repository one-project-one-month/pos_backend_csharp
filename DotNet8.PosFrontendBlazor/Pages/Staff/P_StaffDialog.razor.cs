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
            var response = await HttpClientService.ExecuteAsync<StaffResponseModel>(
                Endpoints.Staff,
                EnumHttpMethod.Post,
                reqModel
            );
            if (response.IsError)
            {
                InjectService.ShowMessage(response.Message, EnumResponseType.Error);
                return;
            }

            InjectService.ShowMessage(response.Message, EnumResponseType.Success);
            MudDialog.Close();
        }
    }
}