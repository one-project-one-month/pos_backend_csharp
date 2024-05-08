namespace DotNet8.PosFrontendBlazor.Pages.Township
{
    public partial class P_TownshipDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string townshipCode { get; set; }

        private TownshipModel reqModel = new();

        private TownshipListResponseModel lstStateCode = new TownshipListResponseModel();

        protected override async void OnInitialized()
        {
            lstStateCode = await HttpClientService.ExecuteAsync<TownshipListResponseModel>(
                Endpoints.Township,
                EnumHttpMethod.Get,
                null
                );

            //InjectService.ShowMessage("Township Code =" + townshipCode, EnumResponseType.Success);
            //if (townshipCode is not null)
            //{
            var tspResponseModel = await HttpClientService.ExecuteAsync<TownshipResponseModel>(
                Endpoints.Township + "/" + "MMR013027",
                EnumHttpMethod.Get,
                null
                );
            if (tspResponseModel is not null && tspResponseModel.Item is not null)
            {
                reqModel.TownshipId = tspResponseModel.Item.TownshipId;
                reqModel.StateCode = tspResponseModel.Item.StateCode;
                reqModel.TownshipCode = tspResponseModel.Item.TownshipCode;
                reqModel.TownshipName = tspResponseModel.Item.TownshipName;
                StateHasChanged();
            }
            //}
        }

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
