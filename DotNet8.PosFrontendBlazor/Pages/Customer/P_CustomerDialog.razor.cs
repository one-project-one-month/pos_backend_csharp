namespace DotNet8.PosFrontendBlazor.Pages.Customer
{
    public partial class P_CustomerDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private CustomerRequestModel requestModel = new();

        private void Cancel() => MudDialog.Cancel();

        private async Task SaveAsync()
        {
            requestModel.DateOfBirth = DateTime.Now;

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
