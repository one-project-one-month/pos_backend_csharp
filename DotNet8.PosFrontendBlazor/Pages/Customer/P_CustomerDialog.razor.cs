namespace DotNet8.PosFrontendBlazor.Pages.Customer
{
    public partial class P_CustomerDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private CustomerRequestModel requestModel = new();
        DateTime? DateOfBirth;

        bool isDisabled = true;

        private void Cancel() => MudDialog.Cancel();

        private async Task SaveAsync()
        {
            requestModel.DateOfBirth = DateOfBirth;
            requestModel.CustomerCode = string.Empty;

            var response = await HttpClientService.ExecuteAsync<CustomerResponseModel>(
                Endpoints.Customer,
                EnumHttpMethod.Post,
                requestModel
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
