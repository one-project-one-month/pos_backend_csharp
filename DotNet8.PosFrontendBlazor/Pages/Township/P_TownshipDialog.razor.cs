namespace DotNet8.PosFrontendBlazor.Pages.Township
{
    public partial class P_TownshipDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private TownshipModel reqModel = new();

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await HttpClientService.ExecuteAsync<TownshipResponseModel>(
                Endpoints.Township,
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
